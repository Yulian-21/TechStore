using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TechStore.Tests.Orders.Models;

public class TestOrder
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("clientId")]
    public int ClientId { get; set; }

    [JsonPropertyName("orderedAt")]
    public DateTime OrderedAt { get; set; }
    
    [JsonPropertyName("completedAt")]
    public DateTime? CompletedAt { get; set; }

    [JsonPropertyName("items")]
    public List<TestOrderItem> Items { get; set; }

    [JsonPropertyName("review")]
    public TestOrderReview Review { get; set; }
}