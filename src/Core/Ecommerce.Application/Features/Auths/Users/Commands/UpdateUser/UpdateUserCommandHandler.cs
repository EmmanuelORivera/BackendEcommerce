using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Features.Auths.Users.Vms;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, AuthResponse>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IAuthService _authServie;

    public UpdateUserCommandHandler
    (
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        IAuthService authServie
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _authServie = authServie;
    }

    public async Task<AuthResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var updateUser = await _userManager.FindByNameAsync(_authServie.GetSessionUser());
        if (updateUser is null)
        {
            throw new BadRequestException("User does not exists");
        }
        updateUser.Name = request.Name;
        updateUser.LastName = request.LastName;
        updateUser.Phone = request.Phone;
        updateUser.AvatarUrl = request.PhotoUrl ?? updateUser.AvatarUrl;

        var result = await _userManager.UpdateAsync(updateUser);

        if (!result.Succeeded)
        {
            throw new Exception("Error trying to update the user");
        }

        var userById = await _userManager.FindByEmailAsync(request.Email!);
        var roles = await _userManager.GetRolesAsync(userById!);

        return new AuthResponse
        {
            Id = userById!.Id,
            Name = userById.Name,
            LastName = userById.LastName,
            Phone = userById.Phone,
            Email = userById.Email,
            Username = userById.UserName,
            Avatar = userById.AvatarUrl,
            Token = _authServie.CreateToken(userById, roles),
            Roles = roles
        };
    }
}