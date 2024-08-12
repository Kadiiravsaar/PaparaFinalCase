namespace Papara.Service.Utilities
{
	public class TokenSettings
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string SecurityKey { get; set; }
        public int TokenValidityInMunitues { get; set; }
    }
}
