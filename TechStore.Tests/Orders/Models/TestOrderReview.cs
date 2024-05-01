using System.Text.Json.Serialization;

namespace TechStore.Tests.Orders.Models;

public class TestOrderReview
{
    [JsonPropertyName("rate")]
    public int Rate { get; set; }

    [JsonPropertyName("comment")]
    public string Comment { get; set; }
}