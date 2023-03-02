using System.ComponentModel.DataAnnotations;

namespace ChatWS.Models.Requests
{
    public class AddContactRequest
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string ContactId { get; set; }
    }
}
