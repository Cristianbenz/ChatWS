
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DB
{
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRequired]
        [BsonElement("text")]
        public string Text { get; set; }

        [BsonElement("createdTime")]
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
    }
}
