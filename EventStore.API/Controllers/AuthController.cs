using EventStore.API.Model.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace EventStore.API.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        #region Private Fields

        private readonly IConfiguration _Configuration;
        private readonly IMediator _Mediator;

        #endregion Private Fields

        #region Public Constructors

        public AuthController(IConfiguration configuration, IMediator mediator)
        {
            _Configuration = configuration;
            _Mediator = mediator;
        }

        #endregion Public Constructors

        #region Public Methods

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

        #endregion Public Methods
    }
}