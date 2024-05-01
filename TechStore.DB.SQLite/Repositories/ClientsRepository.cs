using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

using TechStore.DB.Repositories;
using TechStore.DB.SQLite.Contexts;
using TechStore.DB.SQLite.Entities.Users;

using TechStore.Domain.Models.Orders;
using TechStore.Domain.Models.Users;
using TechStore.Domain.Services;
using TechStore.Domain.Types;

namespace TechStore.DB.SQLite.Repositories;

public class ClientsRepository : IClientsRepository
{
    private readonly TechStoreDbContext _context;
    private readonly IMapper _mapper;

    public ClientsRepository(TechStoreDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public bool ClientExistsById(int clientId)
    {
        var query = _context.Clients.AsNoTracking().Where(c => c.Id == clientId);
        return query.Any();
    }

    public bool ClientExistsByEmail(string email)
    {
        var query = _context.Clients.AsNoTracking().Where(c => c.Email == email);
        return query.Any();
    }

    public Client GetClientByEmail(string email)
    {
        var entity = _context.Clients.AsNoTracking().FirstOrDefault(c => c.Email == email);
        return entity == null ? null : _mapper.Map<Client>(entity);
    }
    
    public Client Get(int id)
    {
        var entity = _context.Clients
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Include(c => c.ShippingAddresses)
            .FirstOrDefault();

        return entity is null ? null : _mapper.Map<Client>(entity);
    }

    public IEnumerable<Client> GetAll()
    {
        var entities = _context.Clients.AsNoTracking().ToList();
        return entities.Select(x => _mapper.Map<Client>(x)).ToList();
    }

    public Client Insert(Client model)
    {
        model.Orders = Enumerable.Empty<Order>();

        var entity = _mapper.Map<Client, DbClient>(model);

        _context.Clients.Add(entity);
        _context.SaveChanges();

        return _mapper.Map<Client>(entity);
    }

    public Client Update(int id, Client model)
    {
        var entity = _context.Clients.Find(id);
        if (entity is null) return null;
        
        var update = _mapper.Map<Client, DbClient>(model);

        _context.Entry(entity).CurrentValues.SetValues(update);
        _context.SaveChanges();

        return _mapper.Map<Client>(update);
    }
    
    public void Delete(int id)
    {
        var found = _context.Clients
            .Where(c => c.Id == id)
            .FirstOrDefault();

        if (found is null)
            throw new ArgumentNullException($"Client with id {id} is not found.");

        _context.Clients.Remove(found);
        _context.SaveChanges();
    }

    public bool IsValid(Client entity)
    {
        if (entity.Role != UserRole.Client)
            return false;

        var found = _context.Clients
            .AsNoTracking()
            .Where(a => a.Email == entity.Email)
            .FirstOrDefault();

        if (found is null)
            return false;

        return found.Password == entity.Password;
    }

    public bool CanRegister(Client entity)
    {
        if (entity.Role != UserRole.Client)
            return false;

        var found = _context.Clients
            .AsNoTracking()
            .Where(a => a.Email == entity.Email)
            .FirstOrDefault();

        return found is null;
    }
}
