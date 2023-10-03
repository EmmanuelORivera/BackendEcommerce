using System.Text;
using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Models.Email;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.SendPassword;

public class SendPasswordCommandHandler : IRequestHandler<SendPasswordCommand, string>
{
    private readonly IEmailService _serviceEmail;
    private readonly UserManager<User> _userManager;

    public SendPasswordCommandHandler(UserManager<User> userManager, IEmailService serviceEmail)
    {
        _userManager = userManager;
        _serviceEmail = serviceEmail;
    }

    public async Task<string> Handle(SendPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email!);
        if (user is null)
        {
            throw new BadRequestException("User does not exist");
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var plainTextBytes = Encoding.UTF8.GetBytes(token);
        token = Convert.ToBase64String(plainTextBytes);

        var emailMessage = new EmailMessage
        {
            To = request.Email,
            Body = "To restore the password, click here:",
            Subject = "Restore your password"

        };

        var result = await _serviceEmail.SendEmail(emailMessage, token);

        if (!result)
        {
            throw new Exception("Error trying to send the email");
        }

        return $"The email was sended to the account {request.Email}";
    }
}