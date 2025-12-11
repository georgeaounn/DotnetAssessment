using Application.Abstractions.CQRS;
using Application.Common;
using Application.Features.Items.Commands.CreateItem;
using Application.Features.Items.Commands.CreateItem.Dtos;
using Application.Features.Items.Dtos;
using Application.Features.Items.Queries.GetAllItems;
using Application.Features.Items.Queries.GetAllItems.Dtos;
using Application.Features.Items.Queries.GetItemById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAssessment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly ICommandDispatcher _commands;
        private readonly IQueryDispatcher _queries;

        public ItemsController(ICommandDispatcher commands, IQueryDispatcher queries)
        {
            _commands = commands;
            _queries = queries;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Result<PaginationResult<ItemDto>>>> GetAll([FromQuery] GetAllItemsRequest Request, CancellationToken ct)
        {
            var result = await _queries.Dispatch(new GetAllItemsQuery(Request), ct);

            if(result.IsFailure)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<ActionResult<Result<ItemDto>>> GetById([FromRoute] Guid id, CancellationToken ct)
        {
            var result = await _queries.Dispatch(new GetItemByIdQuery(id), ct);

            if (result.IsFailure)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "SuperAdminOnly")]
        public async Task<ActionResult<Result<ItemDto>>> Create([FromBody] CreateItemRequest Request, CancellationToken ct)
        {
            var result = await _commands.Dispatch(new CreateItemCommand(Request), ct);
            if (result.IsFailure)
                return BadRequest(result);
            return Ok(result);
        }
    }
}