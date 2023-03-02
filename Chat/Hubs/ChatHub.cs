using Microsoft.AspNetCore.SignalR;

namespace ChatWS.Hubs
{
    public class ChatHub : Hub
    {
        private Dictionary<string, string> activeClients = new Dictionary<string, string>();
        public Task OnConnectedAsync(string userId)
        {
            activeClients.Add(userId, Context.ConnectionId);

            return base.OnConnectedAsync();
        }

        public Task OnDisconnectedAsync(Exception? exception, string userId)
        {
            activeClients.Remove(userId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string chat, string user, string message)
        {
            await Clients.Group(chat).SendAsync("sendMessage", user, message);
        }

        public async Task NewMessage(string chat)
        {
            await Clients.Group(chat).SendAsync("newNotification", 1);
        }
    }
}
