namespace TechStore.DB.SQLite.Entities.Users;

public class DbClientShippingAddress
{
    public int Id { get; set; }
    
    public int ClientId { get; set; }

    public string Address { get; set; }

    public DbClient Client { get; set; }
}
