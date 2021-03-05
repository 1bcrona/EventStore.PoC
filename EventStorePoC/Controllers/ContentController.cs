using System.Threading.Tasks;
using EventStore.PoC.API.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventStore.PoC.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await Task.FromResult(Ok("It works"));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddContentCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}