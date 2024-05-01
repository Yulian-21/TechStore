using TechStore.Domain.Models.Products;

namespace TechStore.DB.Repositories;

public interface ICompaniesRepository : IDataRepository<Company>
{
    bool CompanyExists(int companyId);
    
    Company GetCompanyByName(string companyName);
}