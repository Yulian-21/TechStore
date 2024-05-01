using System;

using TechStore.Business.Exceptions;
using TechStore.DB.Repositories;
using TechStore.Domain.Models.Orders;
using TechStore.Domain.Services;

namespace TechStore.Business.Services;

public class OrderItemsService : IOrderItemsService
{
    private readonly IProductsService _productsService;
    private readonly IOrdersRepository _ordersRepository;
    
    private readonly IOrderItemsRepository _orderItemsRepository;
    
    public OrderItemsService(
        IProductsService productsService,
        IOrdersRepository ordersRepository,
        IOrderItemsRepository orderItemsRepository)
    {
        ArgumentNullException.ThrowIfNull(productsService);
        _productsService = productsService;
        
        ArgumentNullException.ThrowIfNull(ordersRepository);
        _ordersRepository = ordersRepository;
        
        ArgumentNullException.ThrowIfNull(orderItemsRepository);
        _orderItemsRepository = orderItemsRepository;
    }

    public OrderItem CreateOrderItem(OrderItem model)
    {
        var order = _ordersRepository.Get(model.OrderId);
        if (order is null) throw new UnknownModelException("Order", "Id", model.OrderId);

        var product = _productsService.GetProductById(model.ProductId);
        if (product is null) throw new UnknownModelException("Product", "Id", model.ProductId);

        model.Price = product.UnitPrice;

        return _orderItemsRepository.Insert(model);
    }

    public OrderItem UpdateOrderItem(OrderItem model)
    {
        var orderExists = _ordersRepository.OrderExists(model.OrderId);
        if (!orderExists) throw new UnknownModelException("Order", "Id", model.OrderId);
        
        var product = _productsService.GetProductById(model.ProductId);
        if (product is null) throw new UnknownModelException("Product", "Id", model.ProductId);

        var orderItemExists = _orderItemsRepository.OrderItemExists(model.Id);
        if (!orderItemExists) throw new UnknownModelException("Order Item", "Id", model.Id);
        
        model.Price = product.UnitPrice;

        return _orderItemsRepository.Update(model.Id, model);
    }
    
    public void DeleteOrderItem(int orderId, int itemId)
    {
        var orderExists = _ordersRepository.OrderExists(orderId);
        if (!orderExists) throw new UnknownModelException("Order", "Id", orderId);
        
        var orderItemExists = _orderItemsRepository.OrderItemExists(itemId);
        if (!orderItemExists) throw new UnknownModelException("Order Item", "Id", itemId);
        
        _orderItemsRepository.Delete(itemId);
    }
}