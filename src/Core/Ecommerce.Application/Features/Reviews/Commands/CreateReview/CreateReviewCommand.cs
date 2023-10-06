using Ecommerce.Application.Features.Reviews.Queries.Vms;
using MediatR;

namespace Ecommerce.Application.Features.Reviews.Commands.CreateReview;

public class CreateReviewCommand : IRequest<ReviewVm>
{
    public int ProductId { get; set; }
    public string? Name { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
}