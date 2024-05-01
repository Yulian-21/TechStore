using System.Collections.Generic;

using TechStore.Domain.Models;
using TechStore.Domain.Models.Orders;

namespace TechStore.DB.Repositories;

public interface IOrdersRepository : IDataRepository<Order>
{
    bool OrderExists(int orderId);
    
    List<Order> GetByClient(int clientId);
}
