namespace TechStore.Controllers.Models.Orders;

public class ApiOrderItemCreateRequest
{
    public int ProductId { get; set; }

    public int Qty { get; set; }

    public string Comment { get; set; }
}