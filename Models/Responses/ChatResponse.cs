using DB;

namespace ChatWS.Models.Responses
{
    public class ChatResponse
    {
        public int Id { get; set; }
        public ICollection<Message> Messages { get; set; }

        public Object Destinatary { get; set; }
    }
}
