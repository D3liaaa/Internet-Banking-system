namespace InternetBanking.core.Features.Auth.Queries.Responses
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public IEnumerable<string>? Roles { get; set; }
    }

}
