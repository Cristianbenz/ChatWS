using DB;
using ChatWS.Models;
using ChatWS.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;
using ChatWS.Models.Requests;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using ChatWS.Models.Responses;
using ChatWS.Hubs;

namespace ChatWS.Services
{
    public class ChatService
    {
        private readonly AppDbContext _db;
        private readonly ChatHub _hub;

        public ChatService(AppDbContext DbContext, ChatHub hub)
        {
            _db = DbContext;
            _hub = hub;
        }

        public async Task<int> Create(CreateChatRequest request)
        {

            Chat chat = new Chat();
            var result = _db.Chats.Add(chat);
            foreach (var user in request.UsersId)
            {
                var getUser = _db.Users.Where(x => x.Id == user).FirstOrDefault();
                getUser?.Chats.Add(result.Entity);
            }
            await _db.SaveChangesAsync();
            await _hub.AddChat(request.UsersId);
            return result.Entity.Id;
        }

        public List<ChatAccess> GetUserChats(int userId)
        {

            var chats = _db.Chats
                .Include(x => x.Users)
                .Where(chat => chat.Users.Select(user => user.Id).Contains(userId))
                .Include(x => x.Messages.OrderBy(x => x.CreatedTime))
                .Select(chat => new ChatAccess
                {
                    Id= chat.Id,
                    LastMessage = chat.Messages.FirstOrDefault(),
                    Destinatary = chat.Users
                    .Where(user => user.Id != userId)
                    .Select(x => new Destinatary{
                       Id = x.Id,
                       Name = x.Name,
                       Picture = x.Picture
                    })
                    .First()
                })
                .ToList();

            if (chats is null)
            {
                throw new NotExistException("Not exist chats");
            }
            else
            {
                return chats;
            }
        }
    }
}
