namespace TechStore.DB.SQLite.Entities.Users;

public abstract class DbLogin
{
    public int Id { get; set; }
    
    public string Email { get; set; }

    public string Password { get; set; }
}
