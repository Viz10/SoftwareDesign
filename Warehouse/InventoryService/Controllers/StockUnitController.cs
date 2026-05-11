using AutoMapper;
using InventoryService.Application.Commands;
using InventoryService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Shared.DTOs.StockUnitDTO;

namespace InventoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockUnitController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public StockUnitController(IMediator _mediator, IMapper _mapper)
        {
            mediator = _mediator;
            mapper = _mapper;
        }

        /// COMMANDS

        [HttpPost("add-stock-unit")]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> AddStockUnit([FromBody] AddStockUnitRequest addStockUnitRequest, CancellationToken cancellationToken)
        {
            AddStockUnitCommand command = mapper.Map<AddStockUnitCommand>(addStockUnitRequest);

            var result = await mediator.Send(command, cancellationToken);

            return result.IsSuccessful ? Ok(result.Value) : BadRequest(result.GetErrors());
        }

        [HttpPut("update-stock-unit")]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> UpdateItem([FromBody] UpdateStockUnitRequest updateStockUnitRequest, CancellationToken cancellationToken)
        {
            UpdateStockUnitCommand command = mapper.Map<UpdateStockUnitCommand>(updateStockUnitRequest);

            var result = await mediator.Send(command, cancellationToken);

            return result.IsSuccessful ? Ok(result.Value) : BadRequest(result.GetErrors());
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteItem([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new DeleteStockUnitCommand(id), cancellationToken);
            return result.IsSuccessful ? Ok(result.Value) : BadRequest(result.GetErrors());
        }

        /// QUERIES

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? barcode, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetEveryStockUnitQuery(barcode), cancellationToken);
            return result.IsSuccessful ? Ok(result.Value) : NotFound(result.GetErrors());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetStockUnitQuery(id), cancellationToken);
            return result.IsSuccessful ? Ok(result.Value) : NotFound(result.GetErrors());
        }
    }
}
