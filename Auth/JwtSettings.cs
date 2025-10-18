namespace ADOLab.Auth
{
    public class JwtSettings
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public int TokenLifetimeMinutes { get; set; } = 120;
    }
}