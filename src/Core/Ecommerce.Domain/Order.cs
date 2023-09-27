using System.ComponentModel.DataAnnotations.Schema;
using Ecommerce.Domain.Common;

namespace Ecommerce.Domain;
public class Order : BaseDomainModel
{
    public Order()
    {
    }
    public Order(
        string buyerName,
        string buyerEmail,
        OrderAddress orderAddress,
        decimal subtotal,
        decimal total,
        decimal taxes,
        decimal shippingPrice
        )
    {
        BuyerName = buyerEmail;
        BuyerUserName = buyerEmail;
        OrderAddress = orderAddress;
        Subtotal = subtotal;
        Total = total;
        Taxes = taxes;
        ShippingPrice = shippingPrice;
    }
    public string? BuyerName { get; set; }
    public string? BuyerUserName { get; set; }
    public OrderAddress? OrderAddress { get; set; }
    public IReadOnlyList<OrderItem>? OrderItems { get; set; }
    [Column(TypeName = "DECIMAL(10,2)")]
    public decimal Subtotal { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    [Column(TypeName = "DECIMAL(10,2)")]
    public decimal Total { get; set; }
    [Column(TypeName = "DECIMAL(10,2)")]
    public decimal Taxes { get; set; }
    [Column(TypeName = "DECIMAL(10,2)")]
    public decimal ShippingPrice { get; set; }
    public string? PaymentIntentId { get; set; }
    public string? ClientSecret { get; set; }
    public string? StripeApiKey { get; set; }
}