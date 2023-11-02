using Ecommerce.Domain;

namespace Ecommerce.Application.Specifications.Products;

public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(ProductSpecificationParams productParams)
        : base(
            x =>
            (string.IsNullOrEmpty(productParams.Search)
            || x.Name!.Contains(productParams.Search)
            || x.Description!.Contains(productParams.Search)
            ) &&
            (!productParams.CategoryId.HasValue || x.CategoryId == productParams.CategoryId) &&
            (!productParams.MinPrice.HasValue || x.Price >= productParams.MinPrice) &&
            (!productParams.MaxPrice.HasValue || x.Price <= productParams.MaxPrice) &&
            (!productParams.Status.HasValue || x.Status == productParams.Status) &&
            (!productParams.Rating.HasValue || x.Rating == productParams.Rating)

        )
    {
        AddInclude(p => p.Reviews!);
        AddInclude(p => p.Images!);

        ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);

        if (!string.IsNullOrEmpty(productParams.Sort))
        {
            switch (productParams.Sort)
            {
                case "nameASC":
                    AddOrderBy(p => p.Name!);
                    break;
                case "nameDESC":
                    AddOrderByDecending(p => p.Name!);
                    break;
                case "priceASC":
                    AddOrderBy(p => p.Price!);
                    break;
                case "priceDESC":
                    AddOrderByDecending(p => p.Price!);
                    break;
                case "ratingASC":
                    AddOrderBy(p => p.Rating!);
                    break;
                case "ratingDESC":
                    AddOrderByDecending(p => p.Rating!);
                    break;

                default:
                    AddOrderBy(p => p.CreatedDate!);
                    break;
            }
        }
        else
        {
            AddOrderByDecending(p => p.CreatedDate!);
        }
    }

}