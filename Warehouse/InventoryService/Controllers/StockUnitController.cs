using AutoMapper;
using InventoryService.Application.Commands;
using InventoryService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Shared.DTOs.StockUnitDTO;

namespace InventoryService.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class StockUnitController(IMediator _mediator, IMapper _mapper) : ControllerBase
    {
        private readonly IMediator mediator = _mediator;
        private readonly IMapper mapper = _mapper;

        /// COMMANDS

        [HttpPost("add-stock-unit")]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> AddStockUnit([FromBody] AddStockUnitRequest addStockUnitRequest, CancellationToken cancellationToken)
        {
            AddStockUnitCommand command = mapper.Map<AddStockUnitCommand>(addStockUnitRequest);
            var result = await mediator.Send(command, cancellationToken);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update-stock-unit")]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> UpdateItem([FromBody] UpdateStockUnitRequest updateStockUnitRequest, CancellationToken cancellationToken)
        {
            UpdateStockUnitCommand command = mapper.Map<UpdateStockUnitCommand>(updateStockUnitRequest);
            var result = await mediator.Send(command, cancellationToken);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteItem([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new DeleteStockUnitCommand(id), cancellationToken);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        /// QUERIES

        [HttpGet]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> GetAll([FromQuery] int itemId,[FromQuery] string? barcode, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetEveryStockUnitQuery(itemId,barcode), cancellationToken);
            return result.IsSuccessful ? Ok(result) : NotFound(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetStockUnitQuery(id), cancellationToken);
            return result.IsSuccessful ? Ok(result) : NotFound(result);
        }

        [HttpGet("export-stockunits")]
        public async Task<IActionResult> GetExportedStockUnits([FromQuery] string strategyName, [FromQuery] int itemId, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new ExportStockUnitsQuery(strategyName, itemId), cancellationToken);
            return !string.IsNullOrEmpty(result) ? Ok(result) : NotFound("Error exporting items!");
        }
    }
}
