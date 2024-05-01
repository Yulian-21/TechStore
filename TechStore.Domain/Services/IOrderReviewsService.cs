using TechStore.Domain.Models.Orders;

namespace TechStore.Domain.Services;

public interface IOrderReviewsService
{
    OrderReview CreateOrderReview(OrderReview model);

    void DeleteOrderReview(int orderId);
}