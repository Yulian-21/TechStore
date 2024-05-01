using TechStore.Domain.Models.Orders;

namespace TechStore.DB.Repositories;

public interface IOrderReviewsRepository : IDataRepository<OrderReview>
{
    OrderReview GetByOrder(int orderId);
}