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

public class OrderItemsRepository : IOrderItemsRepository
{
    private readonly TechStoreDbContext _context;
    private readonly IMapper _mapper;

    public OrderItemsRepository(TechStoreDbContext context, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(context);
        _context = context;
        
        ArgumentNullException.ThrowIfNull(mapper);
        _mapper = mapper;
    }
    
    public IEnumerable<OrderItem> GetAll()
    {
        throw new System.NotImplementedException();
    }

    public bool OrderItemExists(int orderItemId)
    {
        var query = _context.OrderItems.AsNoTracking().Where(x => x.Id == orderItemId);
        return query.Any();
    }

    public OrderItem Get(int id)
    {
        throw new System.NotImplementedException();
    }

    public OrderItem Insert(OrderItem model)
    {
        var entity = _mapper.Map<DbOrderItem>(model);
        entity.Id = 0;

        _context.OrderItems.Add(entity);
        _context.SaveChanges();

        return _mapper.Map<OrderItem>(entity);
    }

    public OrderItem Update(int id, OrderItem model)
    {
        var entity = _context.OrderItems.Find(id);
        if (entity is null) return null;

        var update = _mapper.Map<DbOrderItem>(model);
        
        _context.Entry(entity).CurrentValues.SetValues(update);
        _context.SaveChanges();

        return _mapper.Map<OrderItem>(update);
    }

    public void Delete(int id)
    {
        var entity = _context.OrderItems.FirstOrDefault(x => x.Id == id);
        if (entity is null) return;

        _context.OrderItems.Remove(entity);
        _context.SaveChanges();
    }
}