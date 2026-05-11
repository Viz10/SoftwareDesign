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
            Email To: {e.AccountEmail}
            Action:   {e.Action}
            Entity:   {e.EntityType}
            Details:  {e.Description}
            Occurred: {e.OccurredAt}";

            var client = _httpFactory.CreateClient("AccountService");
            await client.PostAsJsonAsync("api/account/internal/save-email", new SaveAccountEmailRequest(e.AccountEmail, content), ct);

            Console.WriteLine($"[NOTIFICATION SENT] to {e.AccountEmail}");
            return Unit.Value;
        }
    }
}
