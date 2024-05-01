using TechStore.Domain.Models.Orders;

namespace TechStore.Domain.Services;

public interface IOrderItemsService
{
    OrderItem CreateOrderItem(OrderItem model);
    
    OrderItem UpdateOrderItem(OrderItem model);
    
    void DeleteOrderItem(int orderId, int itemId);
}