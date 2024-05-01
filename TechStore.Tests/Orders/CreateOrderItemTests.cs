using System;
using System.Net;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using TechStore.DB.SQLite.Entities.Orders;
using TechStore.DB.SQLite.Entities.Products;
using TechStore.Tests.Extensions;

using Xunit;

namespace TechStore.Tests.Orders;

[Collection(nameof(TechStoreApiTests))]
public class CreateOrderItemTests : IClassFixture<TechStoreApiFixture>
{
    private readonly TechStoreApiFixture _api;

    public CreateOrderItemTests(TechStoreApiFixture api)
    {
        _api = api;
    }

    [Theory, AutoData]
    public async Task CreateOrderItem_Must_Create_Order_Item(
        decimal productPrice,
        int orderItemQty,
        string orderItemComment)
    {
        await _api.CleanupDb();
        
        var token = await _api.Login();
        var client = Assert.Single(await _api.GetDbClients());

        var companyId = await _api.CreateDbCompany(new DbCompany
        {
            Name = "Company 1"
        });
        
        var productId = await _api.CreateDbProduct(new DbProduct
        {
            Name = "Product 1",
            SupplierId = companyId,
            Price = productPrice
        });

        var orderDate = DateTime.UtcNow;

        var orderId = await _api.CreateDbOrder(new DbOrder
        {
            ClientId = client.Id,
            OrderedAt = orderDate
        });
        
        var (status, _) = await _api.Client.DoPost(token, $"/orders/{orderId}/items", new
        {
            ProductId = productId,
            Qty = orderItemQty,
            Comment = orderItemComment
        });
        
        Assert.Equal(HttpStatusCode.OK, status);

        var orderItem = Assert.Single(await _api.GetDbOrderItems());
        
        Assert.Equal(orderId, orderItem.OrderId);
        Assert.Equal(productId, orderItem.ProductId);
        Assert.Equal(orderItemQty, orderItem.Qty);
        Assert.Equal((double)productPrice, orderItem.Price);
        Assert.Equal(orderItemComment, orderItem.Comment);
    }
    
    [Theory, AutoData]
    public async Task CreateOrderItem_Must_Return_Not_Found_When_Order_Not_Exists(
        decimal productPrice,
        int orderItemQty,
        string orderItemComment)
    {
        await _api.CleanupDb();
        
        var token = await _api.Login();

        var companyId = await _api.CreateDbCompany(new DbCompany
        {
            Name = "Company 1"
        });
        
        var productId = await _api.CreateDbProduct(new DbProduct
        {
            Name = "Product 1",
            SupplierId = companyId,
            Price = productPrice
        });

        var (status, _) = await _api.Client.DoPost(token, "/orders/123/items", new
        {
            ProductId = productId,
            Qty = orderItemQty,
            Comment = orderItemComment
        });
        
        Assert.Equal(HttpStatusCode.NotFound, status);
    }
 
    [Theory, AutoData]
    public async Task CreateOrderItem_Must_Return_Not_Found_When_Product_Not_Exists(
        int orderItemQty,
        string orderItemComment)
    {
        await _api.CleanupDb();
        
        var token = await _api.Login();
        var client = Assert.Single(await _api.GetDbClients());
        
        var orderDate = DateTime.UtcNow;

        var orderId = await _api.CreateDbOrder(new DbOrder
        {
            ClientId = client.Id,
            OrderedAt = orderDate
        });
        
        var (status, _) = await _api.Client.DoPost(token, $"/orders/{orderId}/items", new
        {
            ProductId = 123,
            Qty = orderItemQty,
            Comment = orderItemComment
        });
        
        Assert.Equal(HttpStatusCode.NotFound, status);
    }
}