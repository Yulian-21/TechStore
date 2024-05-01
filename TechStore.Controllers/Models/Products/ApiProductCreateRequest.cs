using System.ComponentModel.DataAnnotations;

namespace TechStore.Controllers.Models.Products;

public class ApiProductCreateRequest
{
    public string Name { get; set; }
    
    public string Description { get; set; }

    public string Model { get; set; }

    public double UnitPrice { get; set; }
    
    public int UnitsAvailable { get; set; }

    public string ProducingCountry { get; set; }

    [Required]
    public int SupplierId { get; set; }
}