using System.Collections.Generic;

using TechStore.DB.SQLite.Entities.Orders;

namespace TechStore.DB.SQLite.Entities.Products;

public class DbProduct
{
    public int Id { get; set; }
    
    public string Name { get; set; }

    public string Description { get; set; }

    public string Model { get; set; }

    public decimal Price { get; set; }

    public int Available { get; set; }

    public string Country { get; set; }

    public int SupplierId { get; set; }

    public DbCompany Supplier { get; set; }

    public List<DbOrderItem> OrderItems { get; set; }

    public IList<DbProductResource> Resources { get; set; }
}
