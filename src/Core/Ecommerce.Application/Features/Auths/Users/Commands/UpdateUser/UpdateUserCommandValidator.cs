using FluentValidation;

namespace Ecommerce.Application.Features.Auths.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(p => p.Name).
            NotEmpty().WithMessage("Name cant be null");

        RuleFor(p => p.LastName).
            NotEmpty().WithMessage("LastName cant be null");
    }
}