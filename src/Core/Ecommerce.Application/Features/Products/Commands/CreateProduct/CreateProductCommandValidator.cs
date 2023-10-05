using FluentValidation;

namespace Ecommerce.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Name cant be null")
            .MaximumLength(50).WithMessage("Name has a limit of 50 characters");

        RuleFor(p => p.Description)
            .NotEmpty().WithMessage("Description cant be null");

        RuleFor(p => p.Stock)
            .NotEmpty().WithMessage("Stock cant be null");

        RuleFor(p => p.Price)
            .NotEmpty().WithMessage("Price cant be null");
    }
}