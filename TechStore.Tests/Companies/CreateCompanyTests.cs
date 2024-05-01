using System.Net;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using Xunit;

using TechStore.DB.SQLite.Entities.Products;

using TechStore.Tests.Extensions;

namespace TechStore.Tests.Companies;

[Collection(nameof(TechStoreApiTests))]
public class CreateCompanyTests : IClassFixture<TechStoreApiFixture>
{
    private readonly TechStoreApiFixture _api;

    public CreateCompanyTests(TechStoreApiFixture api)
    {
        _api = api;
    }

    [Theory]
    [AutoData]
    public async Task CreateCompany_Must_Create_Company_In_Db(
    string companyName,
    TechStoreApiFixture api)
    {
        await api.CleanupDb();

        var token = await api.LoginAdmin();
        var (status, _) = await api.Client.DoPost(token, "/companies", new
        {
            Name = companyName
        });

        Assert.Equal(HttpStatusCode.OK, status);

        var company = Assert.Single(await api.GetDbCompanies());
        Assert.Equal(companyName, company.Name);
    }

    [Fact]
    public async Task CreateCompany_Must_Fail_When_Company_Name_Is_Empty()
    {
        await _api.CleanupDb();

        var token = await _api.LoginAdmin();
        var (status, _) = await _api.Client.DoPost(token, "/companies", new
        {
            Name = string.Empty
        });

        Assert.Equal(HttpStatusCode.BadRequest, status);

        Assert.Empty(await _api.GetDbCompanies());
    }
    
    [Theory, AutoData]
    public async Task CreateCompany_Must_Fail_When_Company_Name_Is_Duplicated(
        string companyName)
    {
        await _api.CleanupDb();

        await _api.CreateDbCompany(new DbCompany { Name = companyName });

        var token = await _api.LoginAdmin();
        var (status, _) = await _api.Client.DoPost(token, "/companies", new
        {
            Name = companyName
        });

        Assert.Equal(HttpStatusCode.BadRequest, status);

        Assert.Single(await _api.GetDbCompanies());
    }
}