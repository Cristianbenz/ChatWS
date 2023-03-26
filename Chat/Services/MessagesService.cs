using ChatWS.Hubs;
using ChatWS.Models.Requests;
using DB;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatWS.Services
{
    public class MessagesService
    {
        private ChatHub _chatHub;
        private AppDbContext _db;
        public MessagesService(AppDbContext context, ChatHub chatHub)
        {
            _db = context;
            _chatHub = chatHub;
        }

        public ICollection<Message>? GetChatMessages(int chatId)
        {
            var messages = _db.Chats.Where(chat => chat.Id == chatId)
                .Include(chat => chat.Messages)
                .Select(chat => chat.Messages)
                .FirstOrDefault();

            if(messages == null)
            {
                throw new Exception();
            }
            else
            {
                return messages;
            }

        }

        public async Task<Message> AddMessage(SendMessageRequest request)
        {
            Message message = new Message()
            {
                Text = request.Message,
                UserId = request.EmisorId,
                ChatId = request.ChatId,
            };
            var messageResult = _db.Messages.Add(message);
            await _db.SaveChangesAsync();
            await _chatHub.SendMessage(request.DestinataryId, message);
            return messageResult.Entity;
        }
    }
}
