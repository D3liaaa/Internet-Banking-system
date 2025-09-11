namespace InternetBanking.core.Features.Auth.Command.Models
{
    public class RegisterRequest
    {
        public string UserName { get; set; }= string.Empty;
        public string Email { get; set; }= string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Role { get; set; } // Optional: "User" or "Admin"
    }
}
