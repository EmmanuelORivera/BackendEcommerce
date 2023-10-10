using System.Net;
using Ecommerce.Application.Features.ShoppingCarts.Queries.GetShoppingCartById;
using Ecommerce.Application.Features.ShoppingCarts.Vms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ShoppingCartController : ControllerBase
{
    private IMediator _mediator;

    public ShoppingCartController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpGet("{id}", Name = "GetShoppingCart")]
    [ProducesResponseType(typeof(ShoppingCartVm), (int)HttpStatusCode.OK)]

    public async Task<ActionResult<ShoppingCartVm>> GetShoppingCart(Guid id)
    {
        var shoppingCartId = id == Guid.Empty ? Guid.NewGuid() : id;
        var query = new GetShoppingCartByIdQuery(shoppingCartId);

        return await _mediator.Send(query);
    }
}