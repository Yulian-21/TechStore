using System.Text.Json.Serialization;

namespace TechStore.Tests.Products.Models;

public class TestProductResource
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
}