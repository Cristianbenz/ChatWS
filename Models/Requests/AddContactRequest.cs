using System.ComponentModel.DataAnnotations;

namespace ChatWS.Models.Requests
{
    public class AddContactRequest
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ContactId { get; set; }
    }
}
