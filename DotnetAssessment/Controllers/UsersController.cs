
using Application.Abstractions.CQRS;
using Application.Features.Users.Commands.RegisterUser;
using Application.Features.Users.Commands.RegisterUser.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAssessment.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ICommandDispatcher _commands;

        public UsersController(ICommandDispatcher commands)
        {
            _commands = commands;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterUserRequest request, CancellationToken ct)
        {
            var result = await _commands.Dispatch(new RegisterUserCommand(request), ct);
            return Ok(result);
        }
    }
}