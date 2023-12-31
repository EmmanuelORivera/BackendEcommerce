using System.Linq.Expressions;
using AutoMapper;
using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Features.Orders.Vms;
using Ecommerce.Application.Models.Payment;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Stripe;

namespace Ecommerce.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderVm>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAuthService _authService;
    private readonly UserManager<User> _userManager;
    private readonly StripeSettings _stripeSettings;

    public CreateOrderCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthService authService, UserManager<User> userManager, IOptions<StripeSettings> stripeSettings)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _authService = authService;
        _userManager = userManager;
        _stripeSettings = stripeSettings.Value;
    }

    public async Task<OrderVm> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var pendingOrder = await _unitOfWork.Repository<Order>().GetEntityAsync(
            x => x.BuyerUserName == _authService.GetSessionUser() && x.Status == OrderStatus.Pending,
            null,
            true
        );

        if (pendingOrder is not null)
        {
            await _unitOfWork.Repository<Order>().DeleteAsync(pendingOrder);
        }

        var includes = new List<Expression<Func<ShoppingCart, object>>>();

        includes.Add(p => p.ShoppingCartItems!.OrderBy(x => x.Product));

        var shoppingCart = await _unitOfWork.Repository<ShoppingCart>().GetEntityAsync(
            x => x.ShoppingCartMasterId == request.ShoppingCartId,
            includes,
            false
        );

        var user = await _userManager.FindByNameAsync(_authService.GetSessionUser());
        if (user is null)
        {
            throw new Exception("User is not authenticated");
        }

        var address = await _unitOfWork.Repository<Ecommerce.Domain.Address>().GetEntityAsync(
            x => x.UserName == user.UserName,
            null,
            false
        );

        OrderAddress orderAddress = new()
        {
            StreetAddress = address.StreetAddress,
            City = address.City,
            PostalCode = address.PostalCode,
            Country = address.Country,
            Line1 = address.Line1,
            UserName = address.UserName
        };

        await _unitOfWork.Repository<OrderAddress>().AddAsync(orderAddress);

        var subtotal = Math.Round(shoppingCart.ShoppingCartItems!.Sum(x => x.Price * x.Quantity), 2);
        var tax = Math.Round(subtotal * Convert.ToDecimal(0.18), 2);
        var shippingPrice = subtotal < 100 ? 10 : 25;
        var total = subtotal + tax + shippingPrice;

        var buyerName = $"{user.Name} {user.LastName}";
        var order = new Order(buyerName, user.UserName!, orderAddress, subtotal, total, tax, shippingPrice);

        await _unitOfWork.Repository<Order>().AddAsync(order);

        var items = new List<OrderItem>();

        foreach (var shoppingElement in shoppingCart.ShoppingCartItems!)
        {
            var orderItem = new OrderItem
            {
                ProductName = shoppingElement.Product,
                ProductId = shoppingElement.ProductId,
                ImageUrl = shoppingElement.Image,
                Price = shoppingElement.Price,
                Quantity = shoppingElement.Quantity,
                OrderId = order.Id
            };

            items.Add(orderItem);
        }

        _unitOfWork.Repository<OrderItem>().AddRange(items);

        var result = await _unitOfWork.Complete();

        if (result <= 0)
        {
            throw new Exception("Error at the moment of create the order");
        }

        StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
        var service = new PaymentIntentService();
        PaymentIntent intent;

        if (string.IsNullOrEmpty(order.PaymentIntentId))
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)order.Total,
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" }
            };

            intent = await service.CreateAsync(options);
            order.PaymentIntentId = intent.Id;
            order.ClientSecret = intent.ClientSecret;
            order.StripeApiKey = _stripeSettings.PublishableKey;
        }
        else
        {
            var options = new PaymentIntentUpdateOptions
            {
                Amount = (long)order.Total
            };
            await service.UpdateAsync(order.PaymentIntentId, options);
        }

        _unitOfWork.Repository<Order>().UpdateEntity(order);

        var orderResult = await _unitOfWork.Complete();

        if (orderResult <= 0)
        {
            throw new Exception("There is an error at the moment of create an intent on stripe");
        }

        return _mapper.Map<OrderVm>(order);
    }
}