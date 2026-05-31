using AutoMapper;
using InventoryService.Application.Commands;
using InventoryService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Shared.DTOs.ItemDTO;

namespace InventoryService.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ItemController(IMediator _mediator, IMapper _mapper) : ControllerBase
    {

        private readonly IMediator mediator = _mediator;
        private readonly IMapper mapper = _mapper;

        /// COMMANDS

        [HttpPost("add-item")]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> AddItem([FromBody] AddItemRequest addItemRequest, CancellationToken cancellationToken)
        {
            AddItemCommand command = mapper.Map<AddItemCommand>(addItemRequest);
            var result = await mediator.Send(command, cancellationToken);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }
        
        [HttpPut("update-item")]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> UpdateItem([FromBody] UpdateItemRequest updateItemRequest, CancellationToken cancellationToken)
        {
            UpdateItemCommand command = mapper.Map<UpdateItemCommand>(updateItemRequest);
            var result = await mediator.Send(command, cancellationToken);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }
        
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteItem([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new DeleteItemCommand(id), cancellationToken);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        /// QUERIES

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? name,[FromQuery] string? sortBy, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetEveryItemQuery(name,sortBy),cancellationToken);
            return result.IsSuccessful ? Ok(result) : NotFound(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetItemQuery(id), cancellationToken);
            return result.IsSuccessful ? Ok(result) : NotFound(result);
        }

        [HttpGet("Name/{id}")]
        public async Task<IActionResult> GetNameById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetStockUnitItemNameQuery(id), cancellationToken);
            return result.IsSuccessful ? Ok(result) : NotFound(result);
        }

        [HttpGet("export-items")]
        public async Task<IActionResult> GetExportedItems([FromQuery] string strategyName, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new ExportItemsQuery(strategyName), cancellationToken);
            return !string.IsNullOrEmpty(result) ? Ok(result) : NotFound("Error exporting items!");
        }
    }
}
