namespace ChattingApp.Services.Auth
{
    public class JwtSettings
    {
        public string JwtSecurityKey { get; set; }
        public string JwtAudience { get; set; }
        public string JwtIssuer { get; set; }
        public int JwtExpiryInDays { get; set; }
    }
}
