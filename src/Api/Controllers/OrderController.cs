using System.Net;
using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Features.Addresses.Command.CreateAddress;
using Ecommerce.Application.Features.Addresses.Vms;
using Ecommerce.Application.Features.Orders.Commands.CreateOrder;
using Ecommerce.Application.Features.Orders.Vms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class OrderController : ControllerBase
{
    private IMediator _mediator;
    private readonly IAuthService _authService;

    public OrderController(IMediator mediator, IAuthService authService)
    {
        _mediator = mediator;
        _authService = authService;
    }

    [HttpPost("address", Name = "CreateAddress")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<AddressVm>> CreateAddress([FromBody] CreateAddressCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost(Name = "CreateOrder")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<OrderVm>> CreateOrder([FromBody] CreateOrderCommand request)
    {
        return await _mediator.Send(request);
    }
}