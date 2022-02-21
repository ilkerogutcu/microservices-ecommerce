using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Features.Queries.Orders.GerOrderDetailsOfCurrentUserQuery;
using Order.Application.Features.Queries.Orders.GetOrderDetailByIdQuery;
using Order.Application.Features.Queries.ViewModels;

namespace Order.API.Controllers
{
    [ApiController]
    [Route("api/v1/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET api/v1/[controller]/{id}
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDetailViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [Authorize(Roles = "Administrator")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetOrderDetailsById(Guid id)
        {
            var result = await _mediator.Send(new GetOrderDetailByIdQuery(id));
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        // GET api/v1/[controller]/{id}
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OrderDetailViewModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [Authorize(Roles = "Buyer")]
        [HttpGet("me")]
        public async Task<IActionResult> GetOrdersOfCurrentUser()
        {
            var result = await _mediator.Send(new GetOrderDetailsOfCurrentUserQuery());
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }
    }
}