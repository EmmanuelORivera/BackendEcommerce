using Ecommerce.Domain;

namespace Ecommerce.Application.Specifications.Products;

public class ProductForCountingSpecification : BaseSpecification<Product>
{
    public ProductForCountingSpecification(ProductSpecificationParams productParams)
        : base(
            x =>
            (string.IsNullOrEmpty(productParams.Search)
            || x.Name!.Contains(productParams.Search)
            || x.Description!.Contains(productParams.Search)
            ) &&
            (!productParams.CategoryId.HasValue || x.CategoryId == productParams.CategoryId) &&
            (!productParams.MinPrice.HasValue || x.Price >= productParams.MinPrice) &&
            (!productParams.MaxPrice.HasValue || x.Price <= productParams.MaxPrice) &&
            (!productParams.Status.HasValue || x.Status == productParams.Status)

        )
    {

    }
}