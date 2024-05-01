namespace TechStore.Domain.Models.Users;

public abstract class LoginDTO
{
    public int Id { get; set; }
    
    public string Email { get; set; }
    
    public string Password { get; set; }
}
