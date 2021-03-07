using EventStore.API.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EventStore.API.Queries;

namespace EventStore.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContentController : ControllerBase
    {
        #region Private Fields

        private readonly IMediator _mediator;

        #endregion Private Fields

        #region Public Constructors

        public ContentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #endregion Public Constructors

        #region Public Methods

        [HttpDelete]
        [Route("{contentId}")]
        public async Task<IActionResult> Delete([FromRoute] DeleteContentCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await Task.FromResult(Ok("It works"));
        }

        [HttpGet]
        [Route("{contentId}")]
        public async Task<IActionResult> Get([FromRoute] ContentDetailQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddContentCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        #endregion Public Methods
    }
}