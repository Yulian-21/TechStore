using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

using TechStore.DB.Repositories;
using TechStore.DB.SQLite.Contexts;
using TechStore.DB.SQLite.Entities.Orders;
using TechStore.Domain.Models.Orders;

namespace TechStore.DB.SQLite.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private readonly TechStoreDbContext _context;
    private readonly IMapper _mapper;

    public OrdersRepository(TechStoreDbContext context, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(context);
        _context = context;
        
        ArgumentNullException.ThrowIfNull(mapper);
        _mapper = mapper;
    }

    public bool OrderExists(int orderId)
    {
        var query = _context.Orders.AsNoTracking().Where(x => x.Id == orderId);
        return query.Any();
    }
    
    public IEnumerable<Order> GetAll()
    {
        var orders = new List<Order>();
        var entities = _context.Orders.AsNoTracking().AsEnumerable();
        foreach (var item in entities)
        {
            var entity = _context.Orders
            .AsNoTracking()
            .Include(x => x.OrderItems)
            .Include(x => x.Reviews)
            .FirstOrDefault(x => x.Id == item.Id);

            orders.Add(_mapper.Map<Order>(entity));
        }
        
        return _mapper.Map<IEnumerable<Order>>(orders);
    }

    public List<Order> GetByClient(int clientId)
    {
        var entities = _context.Orders
            .AsNoTracking()
            .Include(x => x.Reviews)
            .Where(x => x.ClientId == clientId);
        
        return entities.Select(x => _mapper.Map<Order>(x)).ToList();

    }

    public Order Get(int id)
    {
        var entity = _context.Orders
            .AsNoTracking()
            .Include(x => x.OrderItems)
            .Include(x => x.Reviews)
            .FirstOrDefault(x => x.Id == id);
        
        return entity is null ? null : _mapper.Map<Order>(entity);
    }

    public Order Insert(Order model)
    {
        var entity = _mapper.Map<Order, DbOrder>(model);

        _context.Orders.Add(entity);
        _context.SaveChanges();

        return _mapper.Map<Order>(entity);
    }

    public Order Update(int id, Order model)
    {
        var entity = _context.Orders.Find(id);
        if (entity is null) return null;

        var update = _mapper.Map<Order, DbOrder>(model);
        
        _context.Entry(entity).CurrentValues.SetValues(update);
        _context.SaveChanges();

        return _mapper.Map<Order>(update);
    }
    
    public void Delete(int id)
    {
        var reviews = _context.OrderReviews.Where(x => x.OrderId == id).ToList();
        _context.OrderReviews.RemoveRange(reviews);
        
        var items = _context.OrderItems.Where(x => x.OrderId == id).ToList();
        _context.OrderItems.RemoveRange(items);
        
        var order = _context.Orders.FirstOrDefault(x => x.Id == id);
        if (order != null) _context.Orders.RemoveRange(order);

        _context.SaveChanges();
    }
}
