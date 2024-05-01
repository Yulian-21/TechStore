using System.Net;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using Xunit;

using TechStore.Tests.Companies.Models;
using TechStore.Tests.Extensions;

using TechStore.DB.SQLite.Entities.Products;

namespace TechStore.Tests.Companies;

[Collection(nameof(TechStoreApiTests))]
public class GetCompanyTests : IClassFixture<TechStoreApiFixture>
{
    private readonly TechStoreApiFixture _api;

    public GetCompanyTests(TechStoreApiFixture api)
    {
        _api = api;
    }

    [Theory]
    [AutoData]
    public async Task GetCompany_Must_Return_Company_From_Db(
        string companyName)
    {
        await _api.CleanupDb();
        
        var companyId = await _api.CreateDbCompany(new DbCompany { Name = companyName });

        var token = await _api.Login();
        var (status, response) = await _api.Client.DoGet<TestCompany>(token, $"/companies/{companyId}");

        Assert.Equal(HttpStatusCode.OK, status);
        
        Assert.Equal(companyId, response.Id);
        Assert.Equal(companyName, response.Name);
    }

    [Fact]
    public async Task GetCompany_Must_Return_Not_Found_When_Company_Not_Exists()
    {
        await _api.CleanupDb();
        
        var token = await _api.Login();
        var (status, _) = await _api.Client.DoGet(token, "/companies/123");

        Assert.Equal(HttpStatusCode.NotFound, status);
    }
}