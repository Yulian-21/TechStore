namespace TechStore.DB.Configuration;

public class DbConfiguration
{
    public DbType DbType { get; set; }

    public bool UseInMemoryDb { get; set; }
}