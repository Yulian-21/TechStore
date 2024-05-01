using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

using TechStore.DB.Repositories;
using TechStore.DB.SQLite.Contexts;
using TechStore.DB.SQLite.Entities.Users;

using TechStore.Domain.Models.Users;
using TechStore.Domain.Services;
using TechStore.Domain.Types;

namespace TechStore.DB.SQLite.Repositories;

public class AdminRepository : IAdminsRepository
{
    private readonly TechStoreDbContext _context;
    private readonly IMapper _mapper;

    public AdminRepository(TechStoreDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public bool AdminExistsByEmail(string email)
    {
        var query = _context.Admins.AsNoTracking().Where(c => c.Email == email);
        return query.Any();
    }

    public void Delete(int id)
    {
        var found = _context.Admins.Find(id);

        if (found is null)
            throw new ArgumentNullException($"Admin with id {id} is not found.");

        _context.Admins.Remove(found);
        _context.SaveChanges();
    }

    public Admin Get(int id)
    {
        var entity = _context.Admins.Find(id);
        return entity is null ? null : _mapper.Map<Admin>(entity);
    }

    public Admin GetAdminByEmail(string email)
    {
        var entity = _context.Admins.AsNoTracking().FirstOrDefault(c => c.Email == email);
        return entity == null ? null : _mapper.Map<Admin>(entity);
    }

    public IEnumerable<Admin> GetAll()
    {
        var entities = _context.Admins.AsNoTracking().ToList();
        return entities.Select(x => _mapper.Map<Admin>(x)).ToList();
    }

    public Admin Insert(Admin model)
    {
        var entity = _mapper.Map<Admin, DbAdmin>(model);

        _context.Admins.Add(entity);
        _context.SaveChanges();

        return _mapper.Map<Admin>(entity);
    }

    public Admin Update(int id, Admin model)
    {
        model.UserName = model.Email.Split("@")[0];

        var entity = _context.Admins.Find(id);
        if (entity is null) return null;

        var update = _mapper.Map<Admin, DbAdmin>(model);
        
        _context.Entry(entity).CurrentValues.SetValues(update);
        _context.SaveChanges();

        return _mapper.Map<Admin>(entity);
    }

    public bool IsValid(Admin model)
    {
        if (model.Role != UserRole.Admin)
            return false;

        var entity = _context.Admins.AsNoTracking()
            .FirstOrDefault(a => a.Email == model.Email);

        if (entity is null)
            return false;

        return model.Password == entity.Password;
    }

    public bool CanRegister(Admin entity)
    {
        if (entity.Role != UserRole.Admin)
            return false;

        var found = _context.Admins.AsNoTracking().FirstOrDefault(a => a.Email == entity.Email);
        return found is null;
    }
}
