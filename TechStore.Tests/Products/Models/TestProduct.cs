using System.Collections.Generic;
using System.Text.Json.Serialization;

using TechStore.Tests.Companies.Models;

namespace TechStore.Tests.Products.Models;

public class TestProduct
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("unitPrice")]
    public decimal UnitPrice { get; set; }
    
    [JsonPropertyName("unitsAvailable")]
    public int UnitsAvailable { get; set; }

    [JsonPropertyName("producingCountry")]
    public string ProducingCountry { get; set; }

    [JsonPropertyName("images")]
    public IEnumerable<TestProductResource> Images { get; set; }
    
    [JsonPropertyName("documents")]
    public IEnumerable<TestProductResource> Documents { get; set; }
    
    [JsonPropertyName("supplier")]
    public TestCompany Supplier { get; set; }
}