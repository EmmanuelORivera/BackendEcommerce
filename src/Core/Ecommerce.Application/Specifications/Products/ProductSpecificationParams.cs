using Ecommerce.Domain;

namespace Ecommerce.Application.Specifications.Products;

public class ProductSpecificationParams : SpecificationParams
{
    public int? CategoryId { get; set; }
    public decimal? MaxPrice { get; set; }
    public decimal? MinPrice { get; set; }
    public int? Rating { get; set; }
    public ProductStatus? Status { get; set; }
}