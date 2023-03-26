using DB;
using System.ComponentModel.DataAnnotations;

namespace ChatWS.Models.Requests
{
    public class SendMessageRequest
    {
        [Required]
        public int ChatId { get; set; }
        [Required]
        public int EmisorId { get; set; }

        [Required]
        public int DestinataryId { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
