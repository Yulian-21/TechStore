namespace TechStore.DB.SQLite.Entities.Products;

public class DbProductResource
{
    public int Id { get; set; }
    
    public int ProductId { get; set; }

    public string StorageKey { get; set; }

    public string ContentType { get; set; }

    public string Name { get; set; }

    public DbProduct Product { get; set; }
}
