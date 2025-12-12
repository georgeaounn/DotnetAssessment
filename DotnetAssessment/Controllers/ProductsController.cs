using Application.Abstractions.CQRS;
using Application.Common;
using Application.Features.Products.Commands.CreateProduct;
using Application.Features.Products.Commands.CreateProduct.Dtos;
using Application.Features.Products.Commands.DeleteProduct;
using Application.Features.Products.Commands.UpdateProduct;
using Application.Features.Products.Commands.UpdateProduct.Dtos;
using Application.Features.Products.Dtos;
using Application.Features.Products.Queries.GetAllProducts;
using Application.Features.Products.Queries.GetAllProducts.Dtos;
using Application.Features.Products.Queries.GetProductById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAssessment.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ICommandDispatcher _commands;
        private readonly IQueryDispatcher _queries;

        public ProductsController(ICommandDispatcher commands, IQueryDispatcher queries)
        {
            _commands = commands;
            _queries = queries;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Result<List<ProductDto>>>> GetAll([FromQuery] GetAllProductsRequest Request, CancellationToken ct)
        {
            var result = await _queries.Dispatch(new GetAllProductsQuery(Request), ct);
            if (result.IsFailure)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Result<ProductDto>>> GetById([FromRoute] Guid id, CancellationToken ct)
        {
            var result = await _queries.Dispatch(new GetProductByIdQuery(id), ct);

            if (result is null)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "SuperAdminOnly")]
        public async Task<ActionResult<Result<ProductDto>>> Create([FromBody] CreateProductRequest request, CancellationToken ct)
        {
            var result = await _commands.Dispatch(
                new CreateProductCommand(request.Name, request.BasePrice), ct);
            if (result.IsFailure)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = "SuperAdminOnly")]
        public async Task<ActionResult<Result<ProductDto>>> Update([FromRoute] Guid id, [FromBody] UpdateProductRequest request, CancellationToken ct)
        {
            var result = await _commands.Dispatch(
                new UpdateProductCommand(id, request.Name, request.BasePrice, request.IsActive), ct);
            if (result.IsFailure)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "SuperAdminOnly")]
        public async Task<ActionResult<Result>> Delete([FromRoute] Guid id, CancellationToken ct)
        {
            var result = await _commands.Dispatch(new DeleteProductCommand(id), ct);
            if (result.IsFailure)
                return BadRequest(result);

            return Ok(result);
        }
    }

}