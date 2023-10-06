using Ecommerce.Application.Features.Products.Queries.Vms;
using MediatR;

namespace Ecommerce.Application.Features.Products.Commands.ChangeProductStatus;

public class ChangeProductStatusCommand : IRequest<ProductVm>
{
    public int ProductId { get; set; }

    public ChangeProductStatusCommand(int productId)
    {
        ProductId = productId == 0 ? throw new ArgumentException(nameof(productId)) : productId;
    }
}