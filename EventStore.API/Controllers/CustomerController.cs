using System;
using System.Linq;
using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EventStore.API.Commands.Customer;
using EventStore.API.Model;
using EventStore.API.Model.Response;
using EventStore.API.Model.Validation;
using EventStore.API.Queries.Customer;
using EventStore.Domain.Entity;

namespace EventStore.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        [Route("{CustomerId}")]
        public async Task<IActionResult> Delete([FromRoute] DeleteCustomerCommand command)
        {
            var validationResult = await new DeleteCustomerValidator().ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                var message = string.Join(',', validationResult.Errors.Select(x => x.ErrorMessage).ToList());
                throw new ApiException("MODEL_IS_NOT_VALID", message, HttpStatusCode.BadRequest);
            }
            var result = await _mediator.Send(command);
            return Ok(new BaseHttpServiceResponse<bool>() { Data = result });
        }

        [HttpGet]
        [Route("{CustomerId}")]
        public async Task<IActionResult> Get([FromRoute] CustomerDetailQuery query)
        {

            var result = await _mediator.Send(query);
            return Ok(new BaseHttpServiceResponse<Customer>() { Data = result });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddCustomerCommand command)
        {


            var result = await _mediator.Send(command);
            return Created(String.Empty, new BaseHttpServiceResponse<Customer>() { Data = result });
        }

        #endregion Public Methods
    }
}