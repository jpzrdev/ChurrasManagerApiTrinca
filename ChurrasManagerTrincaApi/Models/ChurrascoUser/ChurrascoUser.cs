using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ChurrasManagerTrincaApi.Models
{
    public class ChurrascoUser
    {

        [Required]
        public int? ChurrascoId { get; set; }

        [JsonIgnore]
        public Churrasco Churrasco { get; set; }

        [Required]
        public int? UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        public decimal ValorContribuicao { get; set; }

        [Required]
        public bool BebidaInclusa { get; set; }
        public bool Pago { get; set; }


    }
}