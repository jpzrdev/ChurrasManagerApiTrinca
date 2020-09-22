using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChurrasManagerTrincaApi.Models
{
    public class ChurrascoUpdateDto
    {
        [Required]
        public int? Id { get; set; }

        [Required]
        public DateTime? DataChurras { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "O campo deve ter no máximo 50 caracteres.")]
        public string Motivo { get; set; }

        [MaxLength(200, ErrorMessage = "O campo deve ter no máximo 200 caracteres.")]
        public string Observacoes { get; set; }
        public decimal ValorSugerido { get; set; }
    }
}