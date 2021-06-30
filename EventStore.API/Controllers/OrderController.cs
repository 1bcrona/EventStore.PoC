using EventStore.API.Commands.Order;
using EventStore.API.Model.Response;
using EventStore.API.Model.Response.Dto;
using EventStore.API.Queries.Order;
using EventStore.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EventStore.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        #region Private Fields

        private readonly IMediator _mediator;

        #endregion Private Fields

        #region Public Constructors

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #endregion Public Constructors

        #region Public Methods

        [HttpGet]
        [Authorize]
        [Route("{OrderId}")]
        public async Task<IActionResult> Get([FromRoute] OrderDetailQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(new BaseHttpServiceResponse<OrderDto>() { Data = result });
        }

        [HttpPost]
        [Authorize]
        [Route("{productId}")]
        public async Task<IActionResult> Post([FromRoute] Guid productId, [FromBody] AddOrderCommand command)
        {
            command.ProductId = productId;
            var result = await _mediator.Send(command);
            return Created(String.Empty, new BaseHttpServiceResponse<Order>() { Data = result });
        }

        #endregion Public Methods
    }
}