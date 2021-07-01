using EventStore.API.Commands.Customer;
using EventStore.API.Model;
using EventStore.API.Model.Response;
using EventStore.API.Model.Response.Dto;
using EventStore.API.Model.Validation;
using EventStore.API.Queries.Customer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EventStore.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType(typeof(BaseHttpServiceResponse<object>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(BaseHttpServiceResponse<object>), (int)HttpStatusCode.InternalServerError)]
    public class CustomerController : ControllerBase
    {
        #region Private Fields

        private readonly IMediator _mediator;

        #endregion Private Fields

        #region Public Constructors

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #endregion Public Constructors

        #region Public Methods

        [HttpDelete]
        [Authorize]
        [Route("{CustomerId}")]
        [ProducesResponseType(typeof(BaseHttpServiceResponse<bool>), (int)HttpStatusCode.Accepted)]
        public async Task<IActionResult> Delete([FromRoute] DeleteCustomerCommand command)
        {
            var result = await _mediator.Send(command);
            return Accepted(new BaseHttpServiceResponse<bool> { Data = result });
        }

        [HttpGet]
        [Authorize]
        [Route("{CustomerId}")]
        [ProducesResponseType(typeof(BaseHttpServiceResponse<CustomerDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get([FromRoute] CustomerDetailQuery query)
        {
            var result = await _mediator.Send(query);

            if (result == null) return NotFound();
            return Ok(new BaseHttpServiceResponse<CustomerDto> { Data = result });
        }

        [HttpGet]
        [Authorize]
        [Route("{CustomerId}/orders")]
        [ProducesResponseType(typeof(BaseHttpServiceResponse<List<OrderDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get([FromRoute] CustomerOrderDetailQuery query)
        {
            var result = await _mediator.Send(query);
            if (result == null) return NotFound();
            return Ok(new BaseHttpServiceResponse<List<OrderDto>> { Data = result });
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(BaseHttpServiceResponse<CustomerDto>), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Post([FromBody] AddCustomerCommand command)
        {
            var result = await _mediator.Send(command);
            return Created(String.Empty, new BaseHttpServiceResponse<CustomerDto> { Data = result });
        }

        #endregion Public Methods
    }
}