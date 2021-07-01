using EventStore.API.Commands.Product;
using EventStore.API.Model.Response;
using EventStore.API.Model.Response.Dto;
using EventStore.API.Queries.Product;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace EventStore.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType(typeof(BaseHttpServiceResponse<object>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(BaseHttpServiceResponse<object>), (int)HttpStatusCode.InternalServerError)]
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
        [ProducesResponseType(typeof(BaseHttpServiceResponse<bool>), (int)HttpStatusCode.Accepted)]
        public async Task<IActionResult> Delete([FromRoute] DeleteProductCommand command)
        {
            var result = await _mediator.Send(command);
            return Accepted(new BaseHttpServiceResponse<bool> { Data = result });
        }

        [HttpGet]
        [Authorize]
        [Route("{ProductId}")]
        [ProducesResponseType(typeof(BaseHttpServiceResponse<ProductDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get([FromRoute] ProductDetailQuery query)
        {
            var result = await _mediator.Send(query);
            if (result == null) return NotFound();
            return Ok(new BaseHttpServiceResponse<ProductDto> { Data = result });
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(BaseHttpServiceResponse<ProductDto>), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Post([FromBody] AddProductCommand command)
        {
            var result = await _mediator.Send(command);
            return Created(String.Empty, new BaseHttpServiceResponse<ProductDto> { Data = result });
        }

        #endregion Public Methods
    }
}