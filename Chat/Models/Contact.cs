using DB;

namespace ChatWS.Models
{
    public class Contact
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public Message? LastMessage { get; set; }
    }
}
