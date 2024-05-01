namespace TechStore.Domain.Models.Orders;

public class Order
{
    public int Id { get; set; }
    
    public int ClientId { get; set; }

    public DateTime OrderedAt { get; set; }
    
    public DateTime? CompletedAt { get; set; }

    public List<OrderItem> Items { get; set; }

    public OrderReview Review { get; set; }
}
