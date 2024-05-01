using System.Text.Json.Serialization;

namespace TechStore.Tests.Orders.Models;

public class TestOrderItem
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("productId")]
    public int ProductId { get; set; }

    [JsonPropertyName("qty")]
    public int Qty { get; set; }

    [JsonPropertyName("price")]
    public double Price { get; set; }
    
    [JsonPropertyName("comment")]
    public string Comment { get; set; }
}