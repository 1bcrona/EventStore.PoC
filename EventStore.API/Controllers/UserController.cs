using EventStore.API.Commands.User;
using EventStore.API.Queries.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EventStore.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        #region Private Fields

        private readonly IMediator _mediator;

        #endregion Private Fields

        #region Public Constructors

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #endregion Public Constructors

        #region Public Methods

        [HttpDelete]
        [Route("{userId}")]
        public async Task<IActionResult> Delete([FromRoute] DeleteUserCommand command)
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
        [Route("{userId}")]
        public async Task<IActionResult> Get([FromRoute] UserDetailQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        #endregion Public Methods
    }
}