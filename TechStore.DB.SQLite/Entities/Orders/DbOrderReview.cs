namespace TechStore.DB.SQLite.Entities.Orders;

public class DbOrderReview
{
    public int Id { get; set; }
    
    public int OrderId { get; set; }

    public int Rate { get; set; }

    public string Comment { get; set; }

    public DbOrder Order { get; set; }
}
