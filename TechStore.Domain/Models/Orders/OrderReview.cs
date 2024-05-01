namespace TechStore.Domain.Models.Orders;

public class OrderReview
{
    public int Id { get; set; }
    
    public int OrderId { get; set; }
    
    public int Rate { get; set; }
    
    public string Comment { get; set; }
}
