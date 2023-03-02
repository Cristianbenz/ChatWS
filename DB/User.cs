using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DB
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRequired]
        [BsonElement("picture")]
        public string Picture { get; set; }

        [BsonRequired]
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonRequired]
        [BsonElement("email")]
        public string Email { get; set; }

        [BsonRequired]
        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("contacts")]
        public List<string> Contacts { get; set; } = new List<string>();

        [BsonElement("chats")]
        public List<string> Chats { get; set; } = new List<string>();

        [BsonElement("messages")]
        public List<string> Messages { get; set; } = new List<string>();
    }
}
