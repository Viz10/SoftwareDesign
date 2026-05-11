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
        public async Task<IActionResult> Notify([FromBody] WarehouseEvent e)
        {
            await _mediator.Send(new HandleNotificationCommand(e));
            return Ok();
        }
    }
}
