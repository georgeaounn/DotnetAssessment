
using Application.Abstractions.CQRS;
using Application.Common;
using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.Login.Dtos;
using Application.Features.Auth.Commands.RefreshToken;
using Application.Features.Auth.Commands.RefreshToken.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAssessment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ICommandDispatcher _commands;

        public AuthController(ICommandDispatcher commands) { _commands = commands; }

        [HttpPost("Login")]
        public async Task<ActionResult<Result<LoginResponse>>> Login([FromBody] LoginRequest request,CancellationToken ct)
        {
            var result = await _commands.Dispatch(new LoginCommand(request), ct);
            if(result.IsFailure)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("Refresh")]
        public async Task<ActionResult<Result<LoginResponse>>> Refresh([FromBody] RefreshTokenRequest request,CancellationToken ct)
        {
            var result = await _commands.Dispatch(new RefreshTokenCommand(request.Token, request.RefreshToken), ct);
            if(result.IsFailure)
                return BadRequest(result);
            return Ok(result);
        }
    }
}