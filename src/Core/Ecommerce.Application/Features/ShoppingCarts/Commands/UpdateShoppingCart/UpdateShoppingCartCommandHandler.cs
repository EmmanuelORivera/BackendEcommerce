using System.Linq.Expressions;
using AutoMapper;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Features.ShoppingCarts.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;

namespace Ecommerce.Application.Features.ShoppingCarts.Commands.UpdateShoppingCart;

public class UpdateShoppingCartCommandHandler : IRequestHandler<UpdateShoppingCartCommand, ShoppingCartVm>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public UpdateShoppingCartCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<ShoppingCartVm> Handle(UpdateShoppingCartCommand request, CancellationToken cancellationToken)
    {
        var shoppingCartToUpdate = await _unitOfWork.Repository<ShoppingCart>().GetEntityAsync(
            p => p.ShoppingCartMasterId == request.ShoppingCartId
        );

        if (shoppingCartToUpdate is null)
        {
            throw new NotFoundException(nameof(ShoppingCart), request.ShoppingCartId!);
        }

        var shoppingCartItems = await _unitOfWork.Repository<ShoppingCartItem>().GetAsync(
            x => x.ShoppingCartMasterId == request.ShoppingCartId
        );

        _unitOfWork.Repository<ShoppingCartItem>().DeleteRange(shoppingCartItems);

        var shoppingCartItemsToAdd = _mapper.Map<List<ShoppingCartItem>>(request.ShoppingCartItems);

        shoppingCartItemsToAdd.ForEach(x =>
        {
            x.ShoppingCartId = shoppingCartToUpdate.Id;
            x.ShoppingCartMasterId = request.ShoppingCartId;
        });

        _unitOfWork.Repository<ShoppingCartItem>().AddRange(shoppingCartItemsToAdd);

        var result = await _unitOfWork.Complete();

        if (result <= 0)
        {
            throw new Exception("Error trying to add items the shopping cart");
        }

        var includes = new List<Expression<Func<ShoppingCart, object>>>();
        includes.Add(p => p.ShoppingCartItems!.OrderBy(x => x.Product));

        var shoppingCart = await _unitOfWork.Repository<ShoppingCart>().GetEntityAsync(
            x => x.ShoppingCartMasterId == request.ShoppingCartId,
            includes,
            true
        );

        return _mapper.Map<ShoppingCartVm>(shoppingCart);
    }
}