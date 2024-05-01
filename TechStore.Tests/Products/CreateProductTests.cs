using System.Net;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using Xunit;

using TechStore.DB.SQLite.Entities.Products;
using TechStore.Tests.Extensions;

namespace TechStore.Tests.Products;

[Collection(nameof(TechStoreApiTests))]
public class CreateProductTests : IClassFixture<TechStoreApiFixture>
{
    private readonly TechStoreApiFixture _api;

    public CreateProductTests(TechStoreApiFixture api)
    {
        _api = api;
    }

    [Theory, AutoData]
    public async Task CreateProduct_Must_Create_Product_In_Db(
        string companyName,
        string productName,
        string productDescription,
        string productModel,
        decimal productPrice,
        int productAvailable,
        string productCountry)
    {
        await _api.CleanupDb();

        var company = new DbCompany { Name = companyName };
        var companyId = await _api.CreateDbCompany(company);
        
        var token = await _api.LoginAdmin();
        var (status, _) = await _api.Client.DoPost(token, "/products", new
        {
            Name = productName,
            Description = productDescription,
            Model = productModel,
            UnitPrice = productPrice,
            UnitsAvailable = productAvailable,
            ProducingCountry = productCountry,
            SupplierId = companyId
        });

        Assert.Equal(HttpStatusCode.OK, status);

        var product = Assert.Single(await _api.GetDbProducts());
        Assert.Equal(companyName, company.Name);
        
        Assert.Equal(productName, product.Name);
        Assert.Equal(productDescription, product.Description);
        Assert.Equal(productModel, product.Model);
        Assert.Equal(productPrice, product.Price);
        Assert.Equal(productAvailable, product.Available);
        Assert.Equal(productCountry, product.Country);
    }
    
    [Theory, AutoData]
    public async Task CreateProduct_Must_Return_Not_Found_When_Company_Not_Exists(
        string productName,
        string productDescription,
        string productModel,
        decimal productPrice,
        int productAvailable,
        string productCountry)
    {
        await _api.CleanupDb();

        var token = await _api.LoginAdmin();
        var (status, _) = await _api.Client.DoPost(token, "/products", new
        {
            Name = productName,
            Description = productDescription,
            Model = productModel,
            UnitPrice = productPrice,
            UnitsAvailable = productAvailable,
            ProducingCountry = productCountry,
            SupplierId = 123
        });

        Assert.Equal(HttpStatusCode.NotFound, status);
    }
}