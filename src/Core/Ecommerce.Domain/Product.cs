using Ecommerce.Domain.Common;

namespace Ecommerce.Domain;

public class Product:BaseDomainModel{
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public int Rating { get; set; }
    public string? Seller { get; set; }
    public int Stock { get; set; }
    public ProductStatus Status { get; set; }
    public int CategoryId { get; set; }

}