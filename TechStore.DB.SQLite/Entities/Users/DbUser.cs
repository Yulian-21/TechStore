namespace TechStore.DB.SQLite.Entities.Users;

public abstract class DbUser : DbLogin
{
    public string FirstName { get; set; }

    public string LastName { get; set; }
}
