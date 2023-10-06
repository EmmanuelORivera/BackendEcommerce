using FluentValidation;

namespace Ecommerce.Application.Features.Reviews.Commands.CreateReview;

public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotNull().WithMessage("Name does not allow null values");

        RuleFor(p => p.Comment)
            .NotNull().WithMessage("Comment does not allow null values");

        RuleFor(p => p.Rating)
            .NotNull().WithMessage("Rating does not allow null values");
    }
}