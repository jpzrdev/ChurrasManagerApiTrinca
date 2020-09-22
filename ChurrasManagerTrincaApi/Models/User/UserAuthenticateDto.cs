using System.ComponentModel.DataAnnotations;

namespace ChurrasManagerTrincaApi.Models
{
    public class UserAuthenticateDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}