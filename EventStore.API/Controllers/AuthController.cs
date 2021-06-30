using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EventStore.API.Model.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EventStore.API.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _Configuration;
        private readonly IMediator _Mediator;

        public AuthController(IConfiguration configuration, IMediator mediator)
        {
            _Configuration = configuration;
            _Mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_Configuration.GetSection("AuthenticationSecret")?.Value ?? "Very very long secret key to authenticate");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return Created(String.Empty,
                new BaseHttpServiceResponse<string>() { Data = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }
}
