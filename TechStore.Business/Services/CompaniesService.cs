using System;
using System.Collections.Generic;

using TechStore.Business.Exceptions;
using TechStore.DB.Repositories;
using TechStore.Domain.Models.Products;
using TechStore.Domain.Services;

namespace TechStore.Business.Services;

public class CompaniesService : ICompaniesService
{
    private readonly ICompaniesRepository _companiesRepository;
    
    public CompaniesService(
        ICompaniesRepository companiesRepository)
    {
        ArgumentNullException.ThrowIfNull(companiesRepository);
        _companiesRepository = companiesRepository;
    }

    public bool CompanyExists(int companyId)
    {
        return _companiesRepository.CompanyExists(companyId);
    }

    public IEnumerable<Company> GetAllCompanies()
    {
        var companies = _companiesRepository.GetAll();
        _ = companies ?? throw new UnknownModelException("Companies", "Empty", "list");

        return companies;
    }

    public Company GetCompanyById(int companyId)
    {
        var product = _companiesRepository.Get(companyId);
        _ = product ?? throw new UnknownModelException("Company", "Id", companyId);

        return product;
    }

    public Company CreateCompany(Company company)
    {
        var duplicate = _companiesRepository.GetCompanyByName(company.Name);
        if (duplicate is not null)
        {
            throw new InvalidModelException(new Dictionary<string, string[]>
            {
                ["error"] = new[] { $"Company with name \"{company.Name}\" exists already." }
            });
        };
        
        return _companiesRepository.Insert(company);
    }

    public Company UpdateCompany(Company company)
    {
        var exists = _companiesRepository.Get(company.Id) is not null;
        if (!exists) throw new UnknownModelException("Company", "Id", company.Id);
        
        var duplicate = _companiesRepository.GetCompanyByName(company.Name);
        if (duplicate is not null && duplicate.Id != company.Id)
        {
            throw new InvalidModelException(new Dictionary<string, string[]>
            {
                ["error"] = new[] { $"Company with name \"{company.Name}\" exists already." }
            });
        };

        return _companiesRepository.Update(company.Id, company);
    }

    public void DeleteCompany(int companyId)
    {
        var exists = _companiesRepository.Get(companyId) is not null;
        if (!exists) throw new UnknownModelException("Company", "Id", companyId);
        
        _companiesRepository.Delete(companyId);
    }
}