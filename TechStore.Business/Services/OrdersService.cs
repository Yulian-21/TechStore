using System;
using System.Collections.Generic;

using TechStore.Business.Exceptions;
using TechStore.DB.Repositories;
using TechStore.Domain.Models.Orders;
using TechStore.Domain.Services;

namespace TechStore.Business.Services;

public class OrdersService : IOrdersService
{
    private readonly IClientService _clientsService;
    private readonly IOrdersRepository _ordersRepository;
    
    public OrdersService(
        IClientService clientsService,
        IOrdersRepository ordersRepository)
    {
        ArgumentNullException.ThrowIfNull(clientsService);
        _clientsService = clientsService;
        
        ArgumentNullException.ThrowIfNull(ordersRepository);
        _ordersRepository = ordersRepository;
    }

    public Order GetOrderById(int orderId)
    {
        var order = _ordersRepository.Get(orderId);
        return order ?? throw new UnknownModelException("Order", "Id", orderId);
    }

    public IEnumerable<Order> GetAllOrders()
    {
        var orders = _ordersRepository.GetAll();
        return orders ?? throw new UnknownModelException("Orders", "Isn't", "Exist");
    }

    public List<Order> GetClientOrders(int clientId)
    {
        var exists = _clientsService.ClientExists(clientId);
        if (!exists) throw new UnknownModelException("Client", "Id", clientId);
        
        return _ordersRepository.GetByClient(clientId);
    }

    public Order CreateOrder(string email)
    {
        var client = _clientsService.GetClientByEmail(email);
        if (client is null) throw new UnknownModelException("Client", "Email", email);

        return _ordersRepository.Insert(new Order
        {
            ClientId = client.Id,
            OrderedAt = DateTime.UtcNow
        });
    }

    public Order UpdateOrderAsCompleted(int orderId)
    {
        var order = _ordersRepository.Get(orderId);
        if (order is null) throw new UnknownModelException("Order", "Id", orderId);
        
        order.CompletedAt = DateTime.UtcNow;
        return _ordersRepository.Update(orderId, order);
    }
    
    public void DeleteOrder(int orderId)
    {
        var exists = _ordersRepository.OrderExists(orderId);
        if (!exists) throw new UnknownModelException("Order", "Id", orderId);
        
        _ordersRepository.Delete(orderId);
    }
}