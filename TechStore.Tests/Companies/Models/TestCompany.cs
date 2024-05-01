using System.Text.Json.Serialization;

namespace TechStore.Tests.Companies.Models;

public class TestCompany
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
}