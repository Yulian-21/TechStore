using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

using TechStore.DB.Repositories;
using TechStore.DB.SQLite.Contexts;
using TechStore.DB.SQLite.Entities.Products;
using TechStore.Domain.Models.Products;

namespace TechStore.DB.SQLite.Repositories;

public class CompaniesRepository : ICompaniesRepository
{
    private readonly TechStoreDbContext _context;
    private readonly IMapper _mapper;

    public CompaniesRepository(TechStoreDbContext context, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(context);
        _context = context;
        
        ArgumentNullException.ThrowIfNull(mapper);
        _mapper = mapper;
    }

    public bool CompanyExists(int companyId)
    {
        var query = _context.Companies.AsNoTracking().Where(x => x.Id == companyId);
        return query.Any();
    }
    
    public IEnumerable<Company> GetAll()
    {
        var entities = _context.Companies.AsNoTracking().ToList();
        return entities.Select(x => _mapper.Map<Company>(x)).ToList();
    }

    public Company Get(int id)
    {
        var entity = _context.Companies.Find(id);
        return entity is null ? null : _mapper.Map<Company>(entity);
    }

    public Company Insert(Company model)
    {
        var entity = _mapper.Map<DbCompany>(model);

        _context.Companies.Add(entity);
        _context.SaveChanges();

        return _mapper.Map<Company>(entity);
    }

    public Company Update(int id, Company model)
    {
        var entity = _context.Companies.Find(id);
        if (entity is null) return null;

        var update = _mapper.Map<DbCompany>(model);
        
        _context.Entry(entity).CurrentValues.SetValues(update);
        _context.SaveChanges();

        return _mapper.Map<Company>(update);
    }

    public void Delete(int id)
    {
        var entity = _context.Companies.FirstOrDefault(x => x.Id == id);
        if (entity is null) return;

        _context.Companies.Remove(entity);
        _context.SaveChanges();
    }

    public Company GetCompanyByName(string companyName)
    {
        var entity = _context.Companies.FirstOrDefault(x => x.Name == companyName);
        return entity is null ? null : _mapper.Map<Company>(entity);
    }
}