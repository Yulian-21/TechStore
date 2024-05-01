using System.Net;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using TechStore.DB.SQLite.Entities.Products;

using Xunit;

using TechStore.Tests.Extensions;

namespace TechStore.Tests.Products;

[Collection(nameof(TechStoreApiTests))]
public class DeleteProductTests : IClassFixture<TechStoreApiFixture>
{
    private readonly TechStoreApiFixture _api;

    public DeleteProductTests(TechStoreApiFixture api)
    {
        _api = api;
    }
    
    [Theory, AutoData]
    public async Task DeleteProduct_Must_Delete_Product_From_Db(
        string companyName,
        string productName)
    {
        await _api.CleanupDb();
        
        var company = new DbCompany { Name = companyName };
        var companyId = await _api.CreateDbCompany(company);
        
        var productId = await _api.CreateDbProduct(new DbProduct
        {
            Name = productName,
            SupplierId = companyId
        });
        
        var token = await _api.LoginAdmin();
        var (status, _) = await _api.Client.DoDelete(token, $"/products/{productId}");
        
        Assert.Equal(HttpStatusCode.NoContent, status);

        Assert.Empty(await _api.GetDbProducts());
    }
    
    [Fact]
    public async Task DeleteProduct_Must_Return_Not_Found_When_Product_Not_Exists()
    {
        await _api.CleanupDb();

        var token = await _api.LoginAdmin();
        var (status, _) = await _api.Client.DoDelete(token, "/products/123");

        Assert.Equal(HttpStatusCode.NotFound, status);
    }
}