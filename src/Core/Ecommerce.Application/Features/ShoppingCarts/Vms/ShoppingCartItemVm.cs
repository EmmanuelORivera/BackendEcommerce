namespace Ecommerce.Application.Features.ShoppingCarts.Vms;

public class ShoppingCartItemVm
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? Product { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string? Category { get; set; }
    public int Stock { get; set; }
    public decimal TotalLine
    {
        get
        {
            return Math.Round(Quantity * Price, 2);
        }
        set
        {

        }
    }
}