namespace TechStore.Domain.Models.Users;

public abstract class UserDTO : LoginDTO
{
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string Role { get; set; }
}
