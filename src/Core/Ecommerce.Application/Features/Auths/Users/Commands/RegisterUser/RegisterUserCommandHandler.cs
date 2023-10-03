using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Features.Auths.Users.Vms;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResponse>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IAuthService _authService;

    public RegisterUserCommandHandler(IAuthService authService, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
    {
        _authService = authService;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existsUserByEmail = await _userManager.FindByEmailAsync(request.Email!) is null ? false : true;

        if (existsUserByEmail)
        {
            throw new BadRequestException("User email already exists");
        }

        var existsUserByUserName = await _userManager.FindByNameAsync(request.Username!) is null ? false : true;

        if (existsUserByUserName)
        {
            throw new BadRequestException("The username has been already taken");
        }

        var user = new User
        {
            Name = request.Name,
            LastName = request.LastName,
            Phone = request.Phone,
            Email = request.Email,
            UserName = request.Username,
            AvatarUrl = request.PhotoUrl
        };

        var result = await _userManager.CreateAsync(user!, request.Password!);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, AppRole.GenericUser);
            var roles = await _userManager.GetRolesAsync(user);
            return new AuthResponse
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Phone = user.Phone,
                Email = user.Email,
                Username = user.UserName,
                Avatar = user.AvatarUrl,
                Token = _authService.CreateToken(user, roles),
                Roles = roles
            };
        }
        throw new Exception("Error trying to register the user");
    }
}