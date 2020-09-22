using System;
using System.Collections.Generic;

namespace ChurrasManagerTrincaApi.Models
{
    public class ChurrascoGetDto
    {
        public int Id { get; set; }
        public DateTime? DataChurras { get; set; }
        public string Motivo { get; set; }
        public string Observacoes { get; set; }
        public int TotalConvidados { get; set; }
        public decimal TotalArrecadado { get; set; }
        public decimal ValorSugerido { get; set; }
        public decimal ValorSugeridoSemBebida { get; set; }
        public ICollection<ChurrascoUserGetUserDto> Convidados { get; set; }
    }
}