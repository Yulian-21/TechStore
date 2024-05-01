using TechStore.Domain.Models.Orders;

namespace TechStore.Domain.Services;

public interface IOrdersService
{
    Order GetOrderById(int orderId);

    List<Order> GetClientOrders(int clientId);

    IEnumerable<Order> GetAllOrders();

    Order CreateOrder(string email);

    Order UpdateOrderAsCompleted(int orderId);
    
    void DeleteOrder(int orderId);
}