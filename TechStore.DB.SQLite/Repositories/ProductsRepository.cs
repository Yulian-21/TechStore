using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

using TechStore.DB.Repositories;
using TechStore.DB.SQLite.Contexts;
using TechStore.DB.SQLite.Entities.Products;
using TechStore.Domain.Models.Products;

namespace TechStore.DB.SQLite.Repositories;

public class ProductsRepository : IProductsRepository
{
    private readonly TechStoreDbContext _context;
    private readonly IMapper _mapper;

    public ProductsRepository(
        TechStoreDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public bool ProductExists(int productId)
    {
        var query = _context.Products.AsNoTracking().Where(x => x.Id == productId);
        return query.Any();
    }

    public IEnumerable<Product> GetAll()
    {
        var entities = _context.Products.AsNoTracking().ToList();
        return entities.Select(x => _mapper.Map<Product>(x)).ToList();
    }

    public Product Get(int id)
    {
        var entity = _context.Products.AsNoTracking().Include(p => p.Supplier).FirstOrDefault(p => p.Id == id);
        return entity is null ? null : _mapper.Map<Product>(entity);
    }

    public Product Insert(Product model)
    {
        var entity = _mapper.Map<Product, DbProduct>(model);
        entity.Supplier = _context.Companies.Find(model.Supplier.Id);

        _context.Products.Add(entity);
        _context.SaveChanges();

        return _mapper.Map<Product>(entity);
    }

    public Product Update(int id, Product model)
    {
        var entity = _context.Products.Find(id);
        if (entity is null) return null;

        var update = _mapper.Map<Product, DbProduct>(model);
        
        _context.Entry(entity).CurrentValues.SetValues(update);
        _context.SaveChanges();

        return _mapper.Map<Product>(update);
    }
    
    public void Delete(int id)
    {
        var entity = _context.Products.FirstOrDefault(p => p.Id == id);
        if (entity is null) return;

        _context.Products.Remove(entity);
        _context.SaveChanges();
    }
}
