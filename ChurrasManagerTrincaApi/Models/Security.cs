namespace ChurrasManagerTrincaApi.Models
{
    public class Security
    {
        public string Secret { get; set; }
        public int ExpiracaoToken { get; set; }
        public string Emissor { get; set; }
        public string UrlValida { get; set; }
    }
}