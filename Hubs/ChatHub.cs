using ChatWS.Models;
using ChatWS.Models.Requests;
using DB;
using Microsoft.AspNetCore.SignalR;

namespace ChatWS.Hubs
{
    public class ChatHub : Hub
    {
        private Dictionary<int, string> activeClients = new Dictionary<int, string>();
        public void AddUser(int userId)
        {
            if (activeClients.ContainsKey(userId) == false)
            {
                activeClients.Add(userId, Context.ConnectionId);
            } else
            {
                activeClients.Remove(userId);
                activeClients.Add(userId, Context.ConnectionId);
            }
        }

        public async Task AddChat(List<int> users)
        {
            if (activeClients.ContainsKey(users[0]) == true && activeClients.ContainsKey(users[1]) == true)
            {
                await Clients.Clients(new List<string>
                {
                    activeClients[users[0]],
                    activeClients[users[1]]
                })
                 .SendAsync("chatRequest");
            }
        }

        public async Task SendMessage(int destinataryId, Message message)
        {
            if(activeClients.ContainsKey(destinataryId) == true)
            {
                await Clients.Client(activeClients[destinataryId]).SendAsync("receiveMessage", message);
            }
        }
    }
}
