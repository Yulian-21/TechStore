using TechStore.Domain.Models.Orders;

namespace TechStore.DB.Repositories;

public interface IOrderItemsRepository : IDataRepository<OrderItem>
{
    bool OrderItemExists(int orderItemId);
}