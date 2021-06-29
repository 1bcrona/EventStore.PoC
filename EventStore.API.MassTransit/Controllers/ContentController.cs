using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventStore.API.MassTransit.Consumer;
using EventStore.Domain.Entity;
using MassTransit;
using MassTransit.RabbitMqTransport.Integration;

namespace EventStore.API.MassTransit.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContentController : ControllerBase
    {

        private readonly ILogger<ContentController> _Logger;
        private readonly IRequestClient<AddContentCommand> _RequestClient;

        public ContentController(ILogger<ContentController> logger, IRequestClient<AddContentCommand> requestClient)
        {
            _Logger = logger;
            _RequestClient = requestClient;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddContentCommand command)
        {
            var content = await _RequestClient.GetResponse<Product>(command);
            _Logger.Log(LogLevel.Debug, "Command Executed");
            return Ok(content.Message);
        }
    }
}
