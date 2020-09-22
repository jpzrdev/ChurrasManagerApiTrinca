using System.ComponentModel.DataAnnotations;

namespace ChurrasManagerTrincaApi.Models
{
    public class UserUpdateDto
    {
        [Required]
        public int? Id { get; set; }

        [EmailAddress(ErrorMessage = "E-mail em formato inválido.")]
        public string Email { get; set; }
        public string Password { get; set; }
        public string Nome { get; set; }
    }
}