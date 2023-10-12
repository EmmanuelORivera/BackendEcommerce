using Ecommerce.Application.Features.Addresses.Vms;
using Ecommerce.Application.Models.Order;
using Ecommerce.Domain;

namespace Ecommerce.Application.Features.Orders.Vms;

public class OrderVm
{
    public int Id { get; set; }
    public AddressVm? OrderAddress { get; set; }
    public List<OrderItemVm>? OrderItems { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
    public decimal ShippingPrice { get; set; }
    public OrderStatus Status { get; set; }
    public string? PaymentIntentId { get; set; }
    public string? ClientSecret { get; set; }
    public string? StripeApiKey { get; set; }
    public string? BuyerUserName { get; set; }
    public string? BuyerName { get; set; }
    public int Quantity
    {
        get { return OrderItems!.Sum(x => x.Quantity); }
        set { }
    }
    public string StatusLabel
    {
        get
        {
            switch (Status)
            {
                case OrderStatus.Completed:
                    {
                        return OrderStatusLabel.COMPLETED;
                    }
                case OrderStatus.Pending:
                    {
                        return OrderStatusLabel.PENDING;
                    }
                case OrderStatus.Send:
                    {
                        return OrderStatusLabel.SEND;
                    }
                case OrderStatus.Error:
                    {
                        return OrderStatusLabel.ERROR;
                    }
                default: return OrderStatusLabel.ERROR;
            }
        }
    }
}