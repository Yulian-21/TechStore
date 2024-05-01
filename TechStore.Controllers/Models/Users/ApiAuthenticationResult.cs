namespace TechStore.Controllers.Models.Users;

public class ApiAuthenticationResult
{
    public string Username { get; set; }

    public string Token { get; set; }

    public DateTime ExpiresUtc { get; set; }

    public string Role { get; set; }
}