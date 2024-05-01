using TechStore.Domain.Models.Products;

namespace TechStore.Domain.Services;

public interface ICompaniesService
{
    bool CompanyExists(int companyId);
    
    Company GetCompanyById(int companyId);

    IEnumerable<Company> GetAllCompanies();

    Company CreateCompany(Company company);
    
    Company UpdateCompany(Company company);

    void DeleteCompany(int companyId);
}