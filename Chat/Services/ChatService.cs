using DB;
using MongoDB.Driver;
using ChatWS.Models;
using MongoDB.Bson;
using ChatWS.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;
using ChatWS.Models.Requests;

namespace ChatWS.Services
{
    public class ChatService
    {
        private readonly IMongoCollection<Chat> _chatCollection;
        private readonly IMongoCollection<User> _userCollection;
        private readonly IMongoCollection<Message> _messageCollection;

        public ChatService(IDbConfig dbConfig)
        {
            var client = new MongoClient(dbConfig.Server);
            var database = client.GetDatabase(dbConfig.Database);
            _chatCollection = database.GetCollection<Chat>("Chat");
            _messageCollection = database.GetCollection<Message>("Message");
            _userCollection = database.GetCollection<User>("User");
        }

        public string Create([FromBody] CreateChatRequest request)
        {
            Chat chat = new Chat()
            {
                Users = request.UsersId
            };
            _chatCollection.InsertOne(chat);
            foreach (var id in request.UsersId)
            {
                var filterDef = Builders<User>.Filter.Eq("Id", id);
                var user = _userCollection.Find(filterDef).FirstOrDefault();
                user.Chats.Add(chat.Id);
                _userCollection.ReplaceOne(filterDef, user);
            }
            return chat.Id;
        }

        public Chat GetChat(string id)
        {

            var chat = _chatCollection
                .Aggregate()
                .Match(chat => chat.Id == id)
                .Lookup<Chat, Message, Chat>(_messageCollection, chat => chat.Messages, message => message.Id, x => x.Messages)
                .FirstOrDefault();

            if (chat != null)
            {
                return chat;
            }

            throw new NotExistException("Chat not found");
        }
    }
}
