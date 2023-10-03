using FluentValidation;

namespace Ecommerce.Application.Features.Auths.Users.Commands.LoginUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("The email cant be null");

        RuleFor(x => x.Password).NotEmpty().WithMessage("Password cant be null");

    }
}