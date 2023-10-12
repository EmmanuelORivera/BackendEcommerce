namespace Ecommerce.Application.Features.Orders.Vms;

public class OrderItemVm
{
    public int ProductId { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int OrderId { get; set; }
    public int ProductItemId { get; set; }
    public string? ProductName { get; set; }
    public string? ImageUrl { get; set; }
}