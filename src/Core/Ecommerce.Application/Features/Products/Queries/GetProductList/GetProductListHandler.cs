using System.Linq.Expressions;
using AutoMapper;
using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;

namespace Ecommerce.Application.Features.Products.Queries.GetProductList;

public class GetProductListHandler : IRequestHandler<GetProductListQuery, IReadOnlyList<ProductVm>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProductListHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ProductVm>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
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

        var productsVm = _mapper.Map<IReadOnlyList<ProductVm>>(products);

        return productsVm;
    }
}