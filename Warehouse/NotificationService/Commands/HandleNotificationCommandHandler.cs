using MediatR;
using Warehouse.Shared.Common;
using Warehouse.Shared.DTOs.AccountDTO;

namespace NotificationService.Commands
{
    internal record HandleNotificationCommand(WarehouseEvent WarehouseEvent) : IRequest<Unit>;


    internal class HandleNotificationCommandHandler(IHttpClientFactory httpFactory) : IRequestHandler<HandleNotificationCommand, Unit>
    {
        private readonly IHttpClientFactory _httpFactory = httpFactory;

        public async Task<Unit> Handle(HandleNotificationCommand cmd, CancellationToken ct)
        {
            var e = cmd.WarehouseEvent;

            var content =
         $@"Email To: {e.AccountEmail}
            Action:   {e.Action}
            Entity:   {e.EntityType}
            Details:  {e.Description}
            Occurred: {e.OccurredAt}";

            var client = _httpFactory.CreateClient("AccountService");
            await client.PostAsJsonAsync("api/account/internal/save-email", new SaveAccountEmailRequest(e.AccountEmail, content), ct);

            return Unit.Value;
        }
    }
}
