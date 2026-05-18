using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Commands;
using Warehouse.Shared.Common;

namespace NotificationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotifyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotifyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
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
