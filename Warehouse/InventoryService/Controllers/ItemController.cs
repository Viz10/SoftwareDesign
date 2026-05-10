using AutoMapper;
using InventoryService.Application.Commands;
using InventoryService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Warehouse.Shared.DTOs.ItemDTO;

namespace InventoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {

        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public ItemController(IMediator _mediator, IMapper _mapper)
        {
            mediator = _mediator;
            mapper = _mapper;
        }

        /// COMMANDS

        [HttpPost("add-item")]
        //[Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> AddItem([FromBody] AddItemRequest addItemRequest, CancellationToken cancellationToken)
        {
            AddItemCommand command = mapper.Map<AddItemCommand>(addItemRequest);

            var result = await mediator.Send(command, cancellationToken);

            return result.IsSuccessful ? Ok(result.Value) : BadRequest(result.GetErrors());
        }
        
        [HttpPut("update-item")]
        //[Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> UpdateItem([FromBody] UpdateItemRequest updateItemRequest, CancellationToken cancellationToken)
        {
            UpdateItemCommand command = mapper.Map<UpdateItemCommand>(updateItemRequest);

            var result = await mediator.Send(command, cancellationToken);

            return result.IsSuccessful ? Ok(result.Value) : BadRequest(result.GetErrors());
        }
        
        [HttpDelete("delete/{id}")]
        //[Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> DeleteItem([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new DeleteItemCommand(id), cancellationToken);
            return result.IsSuccessful ? Ok(result.Value) : BadRequest(result.GetErrors());
        }

        /// QUERIES

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? name,[FromQuery] string? sortBy, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetEveryItemQuery(name,sortBy),cancellationToken);
            return result.IsSuccessful ? Ok(result.Value) : NotFound(result.GetErrors());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetItemQuery(id), cancellationToken);
            return result.IsSuccessful ? Ok(result.Value) : NotFound(result.GetErrors());
        }
    }
}
