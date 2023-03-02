using Amazon.Runtime.Internal;
using ChatWS.Models;
using ChatWS.Models.Exceptions;
using ChatWS.Models.Requests;
using DB;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace ChatWS.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserService(IDbConfig dbConfig)
        {
            var client = new MongoClient(dbConfig.Server);
            var database = client.GetDatabase(dbConfig.Database);
            _userCollection = database.GetCollection<User>("User");
        }

        public List<User> GetUsers(string name)
        {
            var users = _userCollection.Find(user => user.Name == name).ToList();

            if (users.IsNullOrEmpty())
            {
                throw new NotExistException("User not exists");
            }
            else
            {
                return users;
            }
        }

        public void AddContact(AddContactRequest request)
        {
            var user = _userCollection.Find(user => user.Id == request.UserId).FirstOrDefault();
            var contact = _userCollection.Find(user => user.Id == request.ContactId).FirstOrDefault();
            
            if (user != null && contact != null)
            {
                user.Contacts.Add(contact.Id);
                _userCollection.ReplaceOne(user => user.Id == request.UserId, user);
            }

            throw new NotExistException("User or contact not exist");
        }
    }
}
