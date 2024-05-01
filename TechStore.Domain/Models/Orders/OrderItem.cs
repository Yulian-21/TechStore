namespace TechStore.Domain.Models.Orders;

public class OrderItem
{
    public int Id { get; set; }
    
    public int OrderId { get; set; }
    
    public int ProductId { get; set; }

    public int Qty { get; set; }

    public double Price { get; set; }
    
    public string Comment { get; set; }
}