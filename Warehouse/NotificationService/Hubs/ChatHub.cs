using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
 
namespace NotificationService.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user,string message)
        {
            ///calls the "ReceiveMessage" function on all connected clients
            await Clients.All.SendAsync("ReceiveMessage",user, message);
        }
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"[CHAT] Connected: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"[CHAT] Disconnected: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
