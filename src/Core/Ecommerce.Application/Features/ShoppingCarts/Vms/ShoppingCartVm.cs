namespace Ecommerce.Application.Features.ShoppingCarts.Vms;

public class ShoppingCartVm
{
    public string? ShoppingCartId { get; set; }
    public List<ShoppingCatsItemVm>? ShoppingCartItems { get; set; }
    public decimal Total
    {
        get
        {
            return Math.Round(
                ShoppingCartItems!.Sum(x => x.Price * x.Quantity) +
                ShoppingCartItems!.Sum(x => x.Price * x.Quantity) * Convert.ToDecimal(0.18) +
                ((ShoppingCartItems!.Sum(x => x.Price * x.Quantity)) < 100 ? 10 : 25)
                , 2
                );
        }
        set { }
    }
    public int Quantity
    {
        get { return ShoppingCartItems!.Sum(x => x.Quantity); }
        set { }
    }

    public decimal SubTotal
    {
        get { return Math.Round(ShoppingCartItems!.Sum(x => x.Price * x.Quantity), 2); }
    }
    public decimal Tax
    {
        get { return Math.Round(((ShoppingCartItems!.Sum(x => x.Price * x.Quantity)) * Convert.ToDecimal(0.18)), 2); }
        set { }
    }
    public decimal ShippingPrice
    {
        get
        {
            return (ShoppingCartItems!.Sum(x => x.Price * x.Quantity)) < 100 ? 10 : 25;
        }
        set { }
    }
}