using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Order.Application.Features.Commands.Orders.CheckThreeDPaymentCommand;
using Order.Application.Features.Queries.Orders.GerOrderDetailsOfCurrentUserQuery;
using Order.Application.Features.Queries.Orders.GetOrderDetailByIdQuery;
using Order.Application.Features.Queries.Orders.GetPaymentHtmlContentOfOrderQuery;
using Order.Application.Features.Queries.ViewModels;

namespace Order.API.Controllers
{
    [ApiController]
    [Route("api/v1/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public OrdersController(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
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

        // GET api/v1/[controller]/me
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

        // GET api/v1/[controller]/payment
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OrderDetailViewModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        // [Authorize(Roles = "Buyer")]
        [HttpGet("payment")]
        public async Task<IActionResult> GetPaymentHtmlContentOfOrder()
        {
            var result = await _mediator.Send(new GetPaymentHtmlContentOfOrderQuery());
            return result.Success ? base.Content(result.Data.HtmlContent, "text/html") : BadRequest(result.Message);
        }

        // GET api/v1/[controller]/payment
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OrderDetailViewModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost("payment/callback")]
        public async Task<IActionResult> PaymentCallback([FromForm] string status, [FromForm] string paymentId, [FromForm] string conversationData,
            [FromForm] string conversationId, [FromForm] string mdStatus)
        {
            var result = await _mediator.Send(new CheckThreeDPaymentCommand(status, paymentId, conversationData, conversationId, mdStatus));
            return result.Success
                ? Redirect(_configuration["PaymentResultSettings:SuccessUrl"])
                : Redirect(_configuration["PaymentResultSettings:FailureUrl"]);
        }
    }
}