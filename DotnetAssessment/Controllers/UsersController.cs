
using Application.Abstractions.CQRS;
using Application.Abstractions.Services;
using Application.Common;
using Application.Features.Users.Commands.RegisterUser;
using Application.Features.Users.Commands.RegisterUser.Dtos;
using Application.Features.Users.Queries.GetAllUsers;
using Application.Features.Users.Queries.GetAllUsers.Dtos;
using Application.Features.Users.Queries.GetUserById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAssessment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ICommandDispatcher _commands;
        private readonly ICurrentUser _currentUser;
        private readonly IQueryDispatcher _queries;

        public UsersController(ICommandDispatcher commands, IQueryDispatcher queries, ICurrentUser currentUser)
        {
            _commands = commands;
            _queries = queries;
            _currentUser = currentUser;
        }

        [HttpPost]
        public async Task<ActionResult<Result<UserDto>>> Register(
            [FromBody] RegisterUserRequest request,
            CancellationToken ct)
        {
            var result = await _commands.Dispatch(new RegisterUserCommand(request), ct);
            if(result.IsFailure)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("MyProfile")]
        [Authorize]
        public async Task<ActionResult<Result<UserDto>>> GetMyProfile(CancellationToken ct)
        {
            var result = await _queries.Dispatch(new GetUserByIdQuery(_currentUser.UserId), ct);
            if(result.IsFailure)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Policy = "SuperAdminOnly")]
        public async Task<ActionResult<Result<PaginationResult<UserDto>>>> GetAll(
            [FromQuery] GetAllUsersRequest request,
            CancellationToken ct)
        {
            var result = await _queries.Dispatch(new GetAllUsersQuery(request), ct);
            if(result.IsFailure)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Policy = "SuperAdminOnly")]
        public async Task<ActionResult<Result<UserDto>>> GetById([FromRoute] Guid id, CancellationToken ct)
        {
            var result = await _queries.Dispatch(new GetUserByIdQuery(id), ct);
            if(result.IsFailure)
                return BadRequest(result);
            if(result.Data == null)
                return NotFound(result);
            return Ok(result);
        }
    }
}