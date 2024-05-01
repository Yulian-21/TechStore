using TechStore.Controllers.Models.Companies;

namespace TechStore.Controllers.Models.Products;

public class ApiProduct
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }

    public string Model { get; set; }

    public decimal UnitPrice { get; set; }
    
    public int UnitsAvailable { get; set; }

    public string ProducingCountry { get; set; }

    public List<ApiProductResource> Images { get; set; }
    
    public List<ApiProductResource> Documents { get; set; }
    
    public ApiCompany Supplier { get; set; }
}