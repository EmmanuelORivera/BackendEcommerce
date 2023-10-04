using Ecommerce.Application.Features.Auths.Users.Vms;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Queries.GetByUserId;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, AuthResponse>
{
    private readonly UserManager<User> _userManager;

    public GetUserByIdQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<AuthResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId!);
        if (user is null)
        {
            throw new Exception("User does not exists");
        }

        return new AuthResponse
        {
            Id = user.Id,
            Name = user.Name,
            LastName = user.LastName,
            Phone = user.Phone,
            Email = user.Email,
            Username = user.UserName,
            Avatar = user.AvatarUrl,
            Roles = await _userManager.GetRolesAsync(user)
        };
    }
}