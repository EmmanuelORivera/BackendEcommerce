using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Exceptions;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly IAuthService _authService;

    public ResetPasswordCommandHandler(IAuthService authService, UserManager<User> userManager)
    {
        _authService = authService;
        _userManager = userManager;
    }

    public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var updateUser = await _userManager.FindByNameAsync(_authService.GetSessionUser());
        if (updateUser is null)
        {
            throw new BadRequestException("User does not exists");
        }

        var resultValidateOldPassword = _userManager.PasswordHasher
            .VerifyHashedPassword(updateUser, updateUser.PasswordHash!, request.OldPassword!);

        if (!(resultValidateOldPassword == PasswordVerificationResult.Success))
        {
            throw new BadRequestException("The password is incorrect");
        }

        var hashedNewPassword = _userManager.PasswordHasher.HashPassword(updateUser, request.NewPassword!);
        updateUser.PasswordHash = hashedNewPassword;

        var result = await _userManager.UpdateAsync(updateUser);

        if (!result.Succeeded)
        {
            throw new Exception("There was a problem at the moment of reset the password");
        };

        return Unit.Value;
    }
}