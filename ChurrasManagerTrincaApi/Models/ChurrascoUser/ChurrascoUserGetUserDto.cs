namespace ChurrasManagerTrincaApi.Models
{
    public class ChurrascoUserGetUserDto
    {
        public UserGetDto User { get; set; }
        public decimal ValorContribuicao { get; set; }
        public bool BebidaInclusa { get; set; }
        public bool Pago { get; set; }
    }
}