using System.Net;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using Xunit;

using TechStore.DB.SQLite.Entities.Products;

using TechStore.Tests.Extensions;

namespace TechStore.Tests.Companies;

[Collection(nameof(TechStoreApiTests))]
public class UpdateCompanyTests : IClassFixture<TechStoreApiFixture>
{
    private readonly TechStoreApiFixture _api;

    public UpdateCompanyTests(TechStoreApiFixture api)
    {
        _api = api;
    }

    [Theory]
    [AutoData]
    public async Task UpdateCompany_Must_Update_Company_In_Db(
        string companyName1,
        string companyName2)
    {
        await _api.CleanupDb();

        var companyId = await _api.CreateDbCompany(new DbCompany { Name = companyName1 });

        var token = await _api.LoginAdmin();
        var (status, _) = await _api.Client.DoPut(token, $"/companies/{companyId}", new
        {
            Name = companyName2
        });

        Assert.Equal(HttpStatusCode.OK, status);

        var company = Assert.Single(await _api.GetDbCompanies());
        Assert.Equal(companyName2, company.Name);
    }

    [Theory]
    [AutoData]
    public async Task UpdateCompany_Must_Fail_When_Company_Name_Is_Empty(
        string companyName)
    {
        await _api.CleanupDb();
        
        var companyId = await _api.CreateDbCompany(new DbCompany { Name = companyName });

        var token = await _api.LoginAdmin();
        var (status, _) = await _api.Client.DoPut(token, $"/companies/{companyId}", new
        {
            Name = string.Empty
        });

        Assert.Equal(HttpStatusCode.BadRequest, status);

        var company = Assert.Single(await _api.GetDbCompanies());
        Assert.Equal(companyName, company.Name);
    }
    
    [Theory]
    [AutoData]
    public async Task UpdateCompany_Must_Fail_When_Company_Name_Is_Duplicated(
        string companyName1,
        string companyName2)
    {
        await _api.CleanupDb();
        
        var company1Id = await _api.CreateDbCompany(new DbCompany { Name = companyName1 });
        var company2Id = await _api.CreateDbCompany(new DbCompany { Name = companyName2 });

        var token = await _api.LoginAdmin();
        var (status, _) = await _api.Client.DoPut(token, $"/companies/{company2Id}", new
        {
            Name = companyName1
        });

        Assert.Equal(HttpStatusCode.BadRequest, status);
    }
    
    [Theory]
    [AutoData]
    public async Task UpdateCompany_Must_Fail_When_Company_Not_Exists(
        string companyName)
    {
        await _api.CleanupDb();

        var token = await _api.LoginAdmin();
        var (status, _) = await _api.Client.DoPut(token, "/companies/123", new
        {
            Name = companyName
        });

        Assert.Equal(HttpStatusCode.NotFound, status);
    }
}