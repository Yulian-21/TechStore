namespace TechStore.Domain.Models.Products;

public class Product
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }

    public string Model { get; set; }

    public double UnitPrice { get; set; }
    
    public int UnitsAvailable { get; set; }

    public string ProducingCountry { get; set; }

    public List<ProductResource> Images { get; set; }
    
    public List<ProductResource> Documents { get; set; }
    
    public Company Supplier { get; set; }
}
