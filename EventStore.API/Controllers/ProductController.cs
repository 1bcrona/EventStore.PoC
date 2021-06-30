using EventStore.API.Commands.Product;
using EventStore.API.Model.Response;
using EventStore.API.Queries.Product;
using EventStore.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace EventStore.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        #region Private Fields

        private readonly IMediator _mediator;

        #endregion Private Fields

        #region Public Constructors

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #endregion Public Constructors

        #region Public Methods

        [HttpDelete]
        [Authorize]
        [Route("{ProductId}")]
        public async Task<IActionResult> Delete([FromRoute] DeleteProductCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        [Route("{ProductId}")]
        public async Task<IActionResult> Get([FromRoute] ProductDetailQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(new BaseHttpServiceResponse<Product>() { Data = result });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] AddProductCommand command)
        {
            var result = await _mediator.Send(command);
            return Created(String.Empty, new BaseHttpServiceResponse<Product>() { Data = result });
        }

        #endregion Public Methods
    }
}