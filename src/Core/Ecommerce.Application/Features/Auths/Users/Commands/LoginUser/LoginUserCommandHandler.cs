using AutoMapper;
using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Features.Addresses.Vms;
using Ecommerce.Application.Features.Auths.Users.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthResponse>
{
    private readonly UserManager<User> _userManager;
    private SignInManager<User> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public LoginUserCommandHandler(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        RoleManager<IdentityRole> roleManager,
        IAuthService authService, IMapper mapper,
        IUnitOfWork unitOfWork
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _authService = authService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email!);
        if (user is null)
        {
            throw new NotFoundException(nameof(User), request.Email!);
        }

        if (!user.IsActive)
        {
            throw new Exception("The user is blocked, make contact with the admin.");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password!, false);

        if (!result.Succeeded)
        {
            throw new Exception("Email or Password are incorrect");
        }

        var shippingAddress = await _unitOfWork.Repository<Address>().GetEntityAsync(
             x => x.UserName == user.UserName
         );

        var roles = await _userManager.GetRolesAsync(user);
        var authResponse = new AuthResponse
        {
            Id = user.Id,
            Name = user.Name,
            LastName = user.LastName,
            Email = user.Email,
            Username = user.UserName,
            Avatar = user.AvatarUrl,
            ShippingAddress = _mapper.Map<AddressVm>(shippingAddress),
            Token = _authService.CreateToken(user, roles),
            Roles = roles
        };

        return authResponse;
    }
}