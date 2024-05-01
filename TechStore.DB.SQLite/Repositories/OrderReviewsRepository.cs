using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using TechStore.DB.Repositories;
using TechStore.DB.SQLite.Contexts;
using TechStore.DB.SQLite.Entities.Orders;
using TechStore.Domain.Models.Orders;

namespace TechStore.DB.SQLite.Repositories;

public class OrderReviewsRepository : IOrderReviewsRepository
{
    private readonly TechStoreDbContext _context;
    private readonly IMapper _mapper;
    
    public OrderReviewsRepository(TechStoreDbContext context, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(context);
        _context = context;
        
        ArgumentNullException.ThrowIfNull(mapper);
        _mapper = mapper;
    }
    
    public IEnumerable<OrderReview> GetAll()
    {
        throw new NotSupportedException();
    }

    public OrderReview Get(int id)
    {
        var entity = _context.OrderReviews.FirstOrDefault(x => x.Id == id);
        return entity is null ? null : _mapper.Map<OrderReview>(entity);
    }

    public OrderReview GetByOrder(int orderId)
    {
        var entity = _context.OrderReviews.FirstOrDefault(x => x.OrderId == orderId);
        return entity is null ? null : _mapper.Map<OrderReview>(entity);
    }

    public OrderReview Insert(OrderReview model)
    {
        model.Id = 0;
        var entity = _mapper.Map<DbOrderReview>(model);

        _context.OrderReviews.Add(entity);
        _context.SaveChanges();

        return _mapper.Map<OrderReview>(entity);
    }

    public OrderReview Update(int id, OrderReview model)
    {
        throw new NotSupportedException();
    }

    public void Delete(int id)
    {
        var entity = _context.OrderReviews.FirstOrDefault(x => x.Id == id);
        if (entity is null) return;

        _context.OrderReviews.Remove(entity);
        _context.SaveChanges();
    }
}
