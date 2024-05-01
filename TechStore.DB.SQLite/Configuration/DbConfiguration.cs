namespace TechStore.DB.SQLite.Configuration;

public class DbConfiguration : TechStore.DB.Configuration.DbConfiguration
{
    public string RelativePath { get; set; }

    public string GetConnectionString()
    {
        return $"Data Source={RelativePath}";
    }
}