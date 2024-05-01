using System;
using System.Net;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using Xunit;

using TechStore.DB.SQLite.Entities.Products;

using TechStore.Tests.Extensions;
using TechStore.Tests.Products.Models;

namespace TechStore.Tests.Products;

[Collection(nameof(TechStoreApiTests))]
public class GetProductByIdTests : IClassFixture<TechStoreApiFixture>
{
    private readonly TechStoreApiFixture _api;

    public GetProductByIdTests(TechStoreApiFixture api)
    {
        _api = api;
    }

    [Theory, AutoData]
    public async Task GetProductById_Must_Return_Product_Base_Properties_(
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
        await _api.CreateDbCompany(company);
        
        var productId = await _api.CreateDbProduct(new DbProduct
        {
            Name = productName,
            Description = productDescription,
            Model = productModel,
            Price = productPrice,
            Available = productAvailable,
            Country = productCountry,
            SupplierId = company.Id
        });
        
        var token = await _api.Login();
        var (status, response) = await _api.Client.DoGet<TestProduct>(token, $"/products/{productId}");

        Assert.Equal(HttpStatusCode.OK, status);
        
        Assert.Equal(productId, response.Id);
        Assert.Equal(productName, response.Name);
        Assert.Equal(productDescription, response.Description);
        Assert.Equal(productModel, response.Model);
        Assert.Equal(productPrice, response.UnitPrice);
        Assert.Equal(productAvailable, response.UnitsAvailable);
        Assert.Equal(productCountry, response.ProducingCountry);
    }
    
    [Theory, AutoData]
    public async Task GetProductById_Must_Return_Product_Supplier(
        string companyName,
        string productName)
    {
        await _api.CleanupDb();
        
        var company = new DbCompany { Name = companyName };
        var companyId = await _api.CreateDbCompany(company);
        
        var productId = await _api.CreateDbProduct(new DbProduct
        {
            Name = productName,
            SupplierId = company.Id
        });
        
        var token = await _api.Login();
        var (status, response) = await _api.Client.DoGet<TestProduct>(token, $"/products/{productId}");

        Assert.Equal(HttpStatusCode.OK, status);
        
        Assert.Equal(companyId, response.Supplier?.Id);
        Assert.Equal(companyName, response.Supplier?.Name);
    }
    
    [Theory, AutoData]
    public async Task GetProductById_Must_Return_Product_Resources(
        string companyName,
        string productName,
        string productResource1StorageKey,
        string productResource1Name,
        string productResource2StorageKey,
        string productResource2Name)
    {
        await _api.CleanupDb();
        
        var companyId = await _api.CreateDbCompany(new DbCompany { Name = companyName });
        
        var productId = await _api.CreateDbProduct(new DbProduct
        {
            SupplierId = companyId,
            Name = productName
        });
        
        var productResource1Id = await _api.CreateDbProductResource(new DbProductResource
        {
            ProductId = productId,
            StorageKey = productResource1StorageKey,
            ContentType = "image/png",
            Name = productResource1Name
        });
        
        var productResource2Id = await _api.CreateDbProductResource(new DbProductResource
        {
            ProductId = productId,
            StorageKey = productResource2StorageKey,
            ContentType = "application/pdf",
            Name = productResource2Name
        });
        
        var token = await _api.Login();
        var (status, product) = await _api.Client.DoGet<TestProduct>(token, $"/products/{productId}");

        Assert.Equal(HttpStatusCode.OK, status);

        var productImage = Assert.Single(product.Images);
        Assert.Equal(productResource1Id, productImage.Id);
        Assert.Equal(productResource1Name, productImage.Name);
        
        var productDocument = Assert.Single(product.Documents);
        Assert.Equal(productResource2Id, productDocument.Id);
        Assert.Equal(productResource2Name, productDocument.Name);
    }
    
    [Fact]
    public async Task GetProductById_Must_Return_Not_Found_When_Product_Not_Exists()
    {
        await _api.CleanupDb();
        
        var token = await _api.Login();
        var (status, _) = await _api.Client.DoGet(token, "/products/123");

        Assert.Equal(HttpStatusCode.NotFound, status);
    }
}