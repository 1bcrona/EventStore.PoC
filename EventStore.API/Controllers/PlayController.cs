using EventStore.API.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EventStore.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayController : ControllerBase
    {
        #region Private Fields

        private readonly IMediator _mediator;

        #endregion Private Fields

        #region Public Constructors

        public PlayController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #endregion Public Constructors

        #region Public Methods

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await Task.FromResult(Ok("It works"));
        }

        [HttpPost]
        [Route("{contentId}")]
        public async Task<IActionResult> Post([FromRoute] Guid contentId, [FromBody] PlayContentCommand command)
        {
            command.ContentId = contentId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        #endregion Public Methods
    }
}