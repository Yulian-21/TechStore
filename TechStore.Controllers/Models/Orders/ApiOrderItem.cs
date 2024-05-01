namespace TechStore.Controllers.Models.Orders;

public class ApiOrderItem
{
    public int Id { get; set; }
    
    public int ProductId { get; set; }

    public int Qty { get; set; }

    public double Price { get; set; }
    
    public string Comment { get; set; }
}