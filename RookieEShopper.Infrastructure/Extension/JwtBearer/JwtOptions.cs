namespace RookieEShopper.Infrastructure.Extension.JwtBearer
{
    public class JwtOptions
    {
        public readonly string SectionName = "JwtOptions";
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public int ExpiryMinutes { get; set; }
    }
}