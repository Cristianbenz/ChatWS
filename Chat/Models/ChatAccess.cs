using DB;

namespace ChatWS.Models
{
    public class ChatAccess
    {
        public int Id { get; set; }
        public Destinatary Destinatary { get; set; }
        public Message? LastMessage { get; set; }
    }
}
