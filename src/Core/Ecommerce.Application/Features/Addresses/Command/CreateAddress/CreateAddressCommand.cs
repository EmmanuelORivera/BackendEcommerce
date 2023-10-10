using Ecommerce.Application.Features.Addresses.Vms;
using MediatR;

namespace Ecommerce.Application.Features.Addresses.Command.CreateAddress;

public class CreateAddressCommand : IRequest<AddressVm>
{
    public string? StreetAddress { get; set; }
    public string? City { get; set; }
    public string? Line1 { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
}