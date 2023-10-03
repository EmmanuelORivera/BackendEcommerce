using System.Text;
using Ecommerce.Application.Exceptions;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.ResetPasswordByToken;

public class ResetPasswordByTokenCommandHandler : IRequestHandler<ResetPasswordByTokenCommand, string>
{
    private readonly UserManager<User> _userManager;

    public ResetPasswordByTokenCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<string> Handle(ResetPasswordByTokenCommand request, CancellationToken cancellationToken)
    {
        if (!string.Equals(request.Password, request.ConfirmPassword))
        {
            throw new BadRequestException("The password and its confirmation do not match");
        }
        var updateUser = await _userManager.FindByEmailAsync(request.Email!);

        if (updateUser is null)
        {
            throw new BadRequestException("This email has not a registered user");
        }

        var token = Convert.FromBase64String(request.Token!);
        var tokenResult = Encoding.UTF8.GetString(token);

        var resetedResult = await _userManager.ResetPasswordAsync(updateUser, tokenResult, request.Password!);
        if (!resetedResult.Succeeded)
        {
            throw new Exception("It was an error at the moment of reseting the password");
        }

        return $"The password was succesfully reseted ${request.Email}";
    }
}