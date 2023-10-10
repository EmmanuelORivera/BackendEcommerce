using FluentValidation;

namespace Ecommerce.Application.Features.Addresses.Command.CreateAddress;

public class CreateAddressCommandValidator : AbstractValidator<CreateAddressCommand>
{
    public CreateAddressCommandValidator()
    {
        RuleFor(p => p.StreetAddress)
            .NotEmpty().WithMessage("Street Address can not be null");

        RuleFor(p => p.City)
            .NotEmpty().WithMessage("City can not be null");

        RuleFor(p => p.Line1)
            .NotEmpty().WithMessage("Line1 can not be null");

        RuleFor(p => p.PostalCode)
            .NotEmpty().WithMessage("Postal code can not be null");

        RuleFor(p => p.Country)
            .NotEmpty().WithMessage("Country can not be null");
    }
}