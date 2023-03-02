using ChatWS.Hubs;
using ChatWS.Models;
using ChatWS.Models.Requests;
using DB;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;

namespace ChatWS.Services
{
    public class MessagesService
    {
        private IHubContext<ChatHub> _chatHub;
        private readonly IMongoCollection<Message> _messageCollection;
        private readonly IMongoCollection<Chat> _chatCollection;
        private readonly IMongoCollection<User> _userCollection;
        public MessagesService(IDbConfig dbConfig, IHubContext<ChatHub> chatHub)
        {
            var client = new MongoClient(dbConfig.Server);
            var database = client.GetDatabase(dbConfig.Database);
            _messageCollection = database.GetCollection<Message>("Message");
            _chatCollection = database.GetCollection<Chat>("Chat");
            _userCollection = database.GetCollection<User>("User");
            _chatHub = chatHub;
        }

        public async Task AddMessage(SendMessageRequest request)
        {
            Message message = new Message()
            {
                Text = request.Message
            };
            await _chatHub.Clients.Group(request.ChatId.ToString()).SendAsync("receiveMessage", request.Message);
            _messageCollection.InsertOneAsync(message);
            var chatFilterDef = Builders<Chat>.Filter.Eq("Id", request.ChatId);
            var userFilterDef = Builders<User>.Filter.Eq("Id", request.EmisorId);
            var chat = _chatCollection.Find(chatFilterDef).FirstOrDefault();
            var user = _userCollection.Find(userFilterDef).FirstOrDefault();
            user.Messages.Add(message.Id);
            chat.Messages.Add(message.Id);
            _userCollection.ReplaceOne(userFilterDef, user);
            _chatCollection.ReplaceOne(chatFilterDef, chat);
        }
    }
}
