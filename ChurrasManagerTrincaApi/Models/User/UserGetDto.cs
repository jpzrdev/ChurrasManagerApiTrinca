using System.Collections.Generic;

namespace ChurrasManagerTrincaApi.Models
{
    public class UserGetDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public virtual ICollection<ChurrascoUserGetChurrascoDto> Churrascos { get; set; }
    }
}