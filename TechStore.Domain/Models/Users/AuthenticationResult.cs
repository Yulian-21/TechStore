namespace TechStore.Domain.Models.Users;

public class AuthenticationResult
{
    public string Username { get; set; }

    public string Token { get; set; }

    public DateTime ExpiresUtc { get; set; }
}