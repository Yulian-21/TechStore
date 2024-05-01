namespace TechStore.Controllers.Models.Orders;

public class ApiOrder
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public DateTime OrderedAt { get; set; }
    
    public DateTime? CompletedAt { get; set; }

    public List<ApiOrderItem> Items { get; set; }

    public ApiOrderReview Review { get; set; }
}