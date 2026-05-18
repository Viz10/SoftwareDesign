using MediatR;
using Warehouse.Shared.Common;
using Warehouse.Shared.DTOs.AccountDTO;

namespace NotificationService.Commands
{
    public record HandleNotificationCommand(WarehouseEvent warehouseEvent) : IRequest<Unit>;

    public class HandleNotificationCommandHandler: IRequestHandler<HandleNotificationCommand, Unit>
    {
        private readonly IHttpClientFactory _httpFactory;

        public HandleNotificationCommandHandler(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }
        public async Task<Unit> Handle(HandleNotificationCommand cmd, CancellationToken ct)
        {
            var e = cmd.warehouseEvent;

            var content = $@"
            Email To: {e.AccountEmail}\n
            Action:   {e.Action}\n
            Entity:   {e.EntityType}\n
            Details:  {e.Description}\n
            Occurred: {e.OccurredAt}";

            var client = _httpFactory.CreateClient("AccountService");
            await client.PostAsJsonAsync("api/account/internal/save-email", new SaveAccountEmailRequest(e.AccountEmail, content), ct);

            return Unit.Value;
        }
    }
}
