using System;

using TechStore.Business.Exceptions;
using TechStore.DB.Repositories;
using TechStore.Domain.Models.Orders;
using TechStore.Domain.Services;

namespace TechStore.Business.Services;

public class OrderReviewsService : IOrderReviewsService
{

    private readonly IOrdersRepository _ordersRepository;
    private readonly IOrderReviewsRepository _orderReviewsRepository;
    
    public OrderReviewsService(
        IOrdersRepository ordersRepository,
        IOrderReviewsRepository orderReviewsRepository)
    {
        
        ArgumentNullException.ThrowIfNull(ordersRepository);
        _ordersRepository = ordersRepository;
        
        ArgumentNullException.ThrowIfNull(orderReviewsRepository);
        _orderReviewsRepository = orderReviewsRepository;
    }
    
    public OrderReview CreateOrderReview(OrderReview model)
    {
        var orderExists = _ordersRepository.OrderExists(model.OrderId);
        if (!orderExists) throw new UnknownModelException("Order", "Id", model.OrderId);

        var currentReview = _orderReviewsRepository.GetByOrder(model.OrderId);
        if (currentReview is not null) throw new InvalidModelException("Order has review already.");

        return _orderReviewsRepository.Insert(model);
    }

    public void DeleteOrderReview(int orderId)
    {
        var order = _ordersRepository.Get(orderId);
        if (order is null) throw new UnknownModelException("Order", "Id", orderId);

        var review = _orderReviewsRepository.GetByOrder(orderId);
        if (review is null) throw new UnknownModelException("Order Review", "Order Id", orderId);
        
        _orderReviewsRepository.Delete(review.Id);
    }
}