using FluentValidation;

namespace Ecommerce.Application.Features.Auths.Users.Commands.UpdateAdminUser;

public class UpdateAdminUserCommandValidator : AbstractValidator<UpdateAdminUserCommand>
{
    public UpdateAdminUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name cant be empty");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("LastName cant be empty");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone cant be empty");

    }
}