using System;
using System.Collections.Generic;

using TechStore.DB.SQLite.Entities.Users;

namespace TechStore.DB.SQLite.Entities.Orders;

public class DbOrder
{
    public int Id { get; set; }
    
    public int ClientId { get; set; }

    public DateTime OrderedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public IList<DbOrderReview> Reviews { get; set; }
    
    public DbClient Client { get; set; }
    
    public IList<DbOrderItem> OrderItems { get; set; }
}
