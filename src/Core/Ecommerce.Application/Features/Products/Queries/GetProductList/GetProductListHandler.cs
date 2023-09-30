using System.Linq.Expressions;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;

namespace Ecommerce.Application.Features.Products.Queries.GetProductList;

public class GetProductListHandler : IRequestHandler<GetProductListQuery, List<Product>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProductListHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Product>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
    {
        var includes = new List<Expression<Func<Product, object>>>
        {
            p => p.Images!,
            p => p.Reviews!
        };

        var products = await _unitOfWork.Repository<Product>().GetAsync(
                   null,
                   x => x.OrderBy(y => y.Name),
                   includes,
                   true
               );

        return new List<Product>(products);
    }
}