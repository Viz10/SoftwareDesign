using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Commands;
using Warehouse.Shared.Common;

namespace NotificationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotifyController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> Notify([FromBody] WarehouseEvent warehouseEvent)
        {
            if (warehouseEvent.AccountEmail == null)
            {
                return BadRequest("Account email null");
            }

            await _mediator.Send(new HandleNotificationCommand(warehouseEvent));
            return Ok("Sent Email");
        }
    }
}
