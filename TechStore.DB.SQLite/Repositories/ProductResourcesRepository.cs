using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

using TechStore.DB.Repositories;
using TechStore.DB.SQLite.Contexts;
using TechStore.DB.SQLite.Entities.Products;
using TechStore.Domain.Models.Products;

namespace TechStore.DB.SQLite.Repositories;

public class ProductResourcesRepository : IProductResourcesRepository
{
    private readonly TechStoreDbContext _context;
    private readonly IMapper _mapper;

    public ProductResourcesRepository(TechStoreDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public ProductResource Get(int id)
    {
        var entity = _context.ProductResources.AsNoTracking().FirstOrDefault(p => p.Id == id);
        return entity is null ? null : _mapper.Map<ProductResource>(entity);
    }

    public List<ProductResource> GetProductResources(int productId)
    {
        var entities = _context.ProductResources.Where(x => x.ProductId == productId).ToList();
        return entities.Select(x => _mapper.Map<ProductResource>(x)).ToList();
    }
    
    public void Delete(int id)
    {
        var entity = _context.ProductResources.Find(id);
        if (entity is null) return;

        _context.ProductResources.Remove(entity);
        _context.SaveChanges();
    }

    public IEnumerable<ProductResource> GetAll()
    {
        var entities = _context.ProductResources.AsNoTracking().ToList();
        return entities.Select(x => _mapper.Map<ProductResource>(x)).ToList();
    }

    public ProductResource Insert(ProductResource model)
    {
        var entity = _mapper.Map<DbProductResource>(model);

        _context.ProductResources.Add(entity);
        _context.SaveChanges();

        return _mapper.Map<ProductResource>(entity);
    }

    public ProductResource Update(int id, ProductResource model)
    {
        throw new NotSupportedException();
    }
}
