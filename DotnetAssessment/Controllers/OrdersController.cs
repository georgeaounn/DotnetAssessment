

using Application.Abstractions.CQRS;
using Application.Abstractions.Services;
using Application.Common;
using Application.Features.Orders.Commands.CreateOrder;
using Application.Features.Orders.Commands.CreateOrder.Dtos;
using Application.Features.Orders.Dtos;
using Application.Features.Orders.Queries.GetOrderById;
using Application.Features.Orders.Queries.GetUserOrders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAssessment.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly ICommandDispatcher _commands;
        private readonly IQueryDispatcher _queries;
        private readonly ICurrentUser _currentUser;

        public OrdersController(ICommandDispatcher commands, IQueryDispatcher queries, ICurrentUser currentUser)
        {
            _commands = commands;
            _queries = queries;
            _currentUser = currentUser;
        }

        [HttpPost]
        public async Task<ActionResult<Result<OrderDto>>> Create([FromBody] CreateOrderRequest request, CancellationToken ct)
        {
            var command = new CreateOrderCommand(_currentUser.UserId, request.Items);
            var result = await _commands.Dispatch(command, ct);
            if (result.IsFailure)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Result<OrderDto>>> GetById(Guid id, CancellationToken ct)
        {
            var result = await _queries.Dispatch(new GetOrderByIdQuery(id), ct);
            if (result.IsFailure) 
                return NotFound(result);
            return Ok(result);
        }

        [HttpGet("my-orders")]
        public async Task<ActionResult<List<OrderDto>>> GetMyOrders(CancellationToken ct)
        {
            var result = await _queries.Dispatch(new GetUserOrdersQuery(_currentUser.UserId), ct);
            return Ok(result);
        }

    }
}