using System.ComponentModel.DataAnnotations;

namespace ChatWS.Models.Requests
{
    public class SignUpRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public string Picture { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}