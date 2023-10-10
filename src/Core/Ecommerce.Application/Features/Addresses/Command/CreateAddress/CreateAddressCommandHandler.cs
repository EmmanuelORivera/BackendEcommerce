using AutoMapper;
using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Features.Addresses.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;

namespace Ecommerce.Application.Features.Addresses.Command.CreateAddress;

public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, AddressVm>
{
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateAddressCommandHandler(IAuthService authService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _authService = authService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AddressVm> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        var addressRecord = await _unitOfWork.Repository<Address>().GetEntityAsync(
            x => x.UserName == _authService.GetSessionUser(),
            null,
            false
        );

        if (addressRecord is null)
        {
            addressRecord = new Address
            {
                StreetAddress = request.StreetAddress,
                City = request.City,
                Line1 = request.Line1,
                PostalCode = request.PostalCode,
                UserName = _authService.GetSessionUser()
            };
            _unitOfWork.Repository<Address>().AddEntity(addressRecord);
        }
        else
        {
            addressRecord.StreetAddress = request.StreetAddress;
            addressRecord.City = request.City;
            addressRecord.Line1 = request.Line1;
            addressRecord.PostalCode = request.PostalCode;
            addressRecord.Country = request.Country;
        }

        await _unitOfWork.Complete();

        return _mapper.Map<AddressVm>(addressRecord);
    }
}