namespace TechStore.Controllers.Models.Orders;

public class ApiOrderItemUpdateRequest
{
    public int ProductId { get; set; }

    public int Qty { get; set; }

    public string Comment { get; set; }
}