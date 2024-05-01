using System.Collections.Generic;

using TechStore.DB.SQLite.Entities.Orders;

namespace TechStore.DB.SQLite.Entities.Users;

public class DbClient : DbUser
{
    public IList<DbOrder> Orders { get; set; }
    
    public IList<DbClientShippingAddress> ShippingAddresses { get; set; }
}
