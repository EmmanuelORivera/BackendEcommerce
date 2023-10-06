using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;

namespace Ecommerce.Application.Features.Reviews.Commands.DeleteReview;

public class DeletereviewCommandHandler : IRequestHandler<DeleteReviewCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeletereviewCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        var reviewToDelete = await _unitOfWork.Repository<Review>().GetByIdAsync(request.ReviewId);
        if (reviewToDelete is null)
        {
            throw new NotFoundException(nameof(Review), request.ReviewId);
        }

        _unitOfWork.Repository<Review>().DeleteEntity(reviewToDelete);
        await _unitOfWork.Complete();

        return Unit.Value;
    }
}