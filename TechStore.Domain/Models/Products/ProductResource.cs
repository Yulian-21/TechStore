namespace TechStore.Domain.Models.Products;

public class ProductResource
{
    public int Id { get; set; }
    
    public int ProductId { get; set; }
    
    public string ContentType { get; set; }

    public string Name { get; set; }

    public string StorageKey { get; set; }
}
