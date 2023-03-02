using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class Chat
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRequired]
        [BsonElement("users")]
        public List<string> Users { get; set; } = new List<string>();

        [BsonRequired]
        [BsonElement("messages")]
        public List<string> Messages { get; set; } = new List<string>();
    }
}
