
using Application.Abstractions.CQRS;
using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.Login.Dtos;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAssessment.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ICommandDispatcher _commands;

        public AuthController(ICommandDispatcher commands)
        {
            _commands = commands;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] Application.Features.Auth.Commands.Login.Dtos.LoginRequest request, CancellationToken ct)
        {
            var result = await _commands.Dispatch(new LoginCommand(request), ct);
            return Ok(result);
        }
    }
}