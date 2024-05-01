namespace TechStore.Controllers.Models.Users;

public class ApiClient
{
    public int Id { get; set; }
    
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public List<string> Addresses { get; set; }
}