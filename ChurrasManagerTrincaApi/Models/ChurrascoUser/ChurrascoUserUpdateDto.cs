using System.ComponentModel.DataAnnotations;

namespace ChurrasManagerTrincaApi.Models
{
    public class ChurrascoUserUpdateDto
    {
        [Required]
        public int? ChurrascoId { get; set; }

        [Required]
        public int? UserId { get; set; }
        public decimal ValorContribuicao { get; set; }
        public bool BebidaInclusa { get; set; }

        [Required]
        public bool Pago { get; set; }
    }
}