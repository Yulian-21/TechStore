using System;
using System.Collections.Generic;
using System.Linq;

using TechStore.Business.Exceptions;
using TechStore.DB.Repositories;
using TechStore.Domain.Models.Users;
using TechStore.Domain.Services;
using TechStore.Domain.Services.Authentication;

namespace TechStore.Business.Services;

public class AdminsService : IAdminsService
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IAdminsRepository _adminsRepository;

    public AdminsService(
        IAuthenticationService authenticationService,
        IAdminsRepository adminsRepository)
    {
        ArgumentNullException.ThrowIfNull(authenticationService);
        _authenticationService = authenticationService;
        
        ArgumentNullException.ThrowIfNull(adminsRepository);
        _adminsRepository = adminsRepository;
    }

    public Admin RegisterAdmin(Admin admin)
    {
        var exists = _adminsRepository.AdminExistsByEmail(admin.Email);
        if (exists) throw new InvalidModelException("Admin with the same Email address exists already.");

        admin.Password = _authenticationService.ComputePasswordHash(admin.Password);
        return _adminsRepository.Insert(admin);
    }

    public Admin GetAdminByEmail(string email)
    {
        return _adminsRepository.GetAdminByEmail(email);
    }

    public IEnumerable<Admin> GetAll()
    {
        return _adminsRepository.GetAll();
    }

    public void DeleteById(int id)
    {
        var admin = _adminsRepository.Get(id);
        var exists = _adminsRepository.AdminExistsByEmail(admin.Email);
        if (!exists) throw new InvalidModelException("Admin is not exist");

        _adminsRepository.Delete(admin.Id);
    }
}