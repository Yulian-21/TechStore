using System.Net;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using Xunit;

using TechStore.DB.SQLite.Entities.Products;

using TechStore.Tests.Extensions;

namespace TechStore.Tests.Companies;

[Collection(nameof(TechStoreApiTests))]
public class DeleteCompanyTests : IClassFixture<TechStoreApiFixture>
{
    private readonly TechStoreApiFixture _api;

    public DeleteCompanyTests(TechStoreApiFixture api)
    {
        _api = api;
    }

    [Theory]
    [AutoData]
    public async Task DeleteCompany_Must_Delete_Company_In_Db(
        string companyName)
    {
        await _api.CleanupDb();

        var companyId = await _api.CreateDbCompany(new DbCompany { Name = companyName });

        var token = await _api.LoginAdmin();
        var (status, _) = await _api.Client.DoDelete(token, $"/companies/{companyId}");
        
        Assert.Equal(HttpStatusCode.NoContent, status);

        Assert.Empty(await _api.GetDbCompanies());
    }

    [Fact]
    public async Task UpdateCompany_Must_Fail_When_Company_Not_Exists()
    {
        await _api.CleanupDb();

        var token = await _api.LoginAdmin();
        var (status, _) = await _api.Client.DoDelete(token, $"/companies/123");
        
        Assert.Equal(HttpStatusCode.NotFound, status);
    }
}