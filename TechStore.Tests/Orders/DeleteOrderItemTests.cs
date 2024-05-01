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
public class DeleteOrderItemTests : IClassFixture<TechStoreApiFixture>
{
    private readonly TechStoreApiFixture _api;

    public DeleteOrderItemTests(TechStoreApiFixture api)
    {
        _api = api;
    }

    [Theory, AutoData]
    public async Task DeleteOrderItem_Must_Delete_Order_Item(
        double product1Price,
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
            Price = (decimal)product1Price
        });
        
        var orderDate = DateTime.UtcNow;

        var orderId = await _api.CreateDbOrder(new DbOrder
        {
            ClientId = client.Id,
            OrderedAt = orderDate
        });

        var orderItemId = await _api.CreateDbOrderItem(new DbOrderItem
        {
            OrderId = orderId,
            ProductId = productId,
            Price = product1Price,
            Qty = orderItemQty,
            Comment = orderItemComment
        });
        
        var (status, _) = await _api.Client.DoDelete(token, $"/orders/{orderId}/items/{orderItemId}");
        Assert.Equal(HttpStatusCode.NoContent, status);
        
        Assert.Empty(await _api.GetDbOrderItems());
    }

    [Theory, AutoData]
    public async Task DeleteOrderItem_Must_Return_Not_Found_When_Order_Not_Exists(
        double product1Price,
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
            Price = (decimal)product1Price
        });
        
        var orderDate = DateTime.UtcNow;

        var orderId = await _api.CreateDbOrder(new DbOrder
        {
            ClientId = client.Id,
            OrderedAt = orderDate
        });

        var orderItemId = await _api.CreateDbOrderItem(new DbOrderItem
        {
            OrderId = orderId,
            ProductId = productId,
            Price = product1Price,
            Qty = orderItemQty,
            Comment = orderItemComment
        });
        
        var (status, _) = await _api.Client.DoDelete(token, $"/orders/123/items/{orderItemId}");
        Assert.Equal(HttpStatusCode.NotFound, status);
    }
    
    [Theory, AutoData]
    public async Task DeleteOrderItem_Must_Return_Not_Found_When_Order_Item_Not_Exists(
        double product1Price,
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
            Price = (decimal)product1Price
        });
        
        var orderDate = DateTime.UtcNow;

        var orderId = await _api.CreateDbOrder(new DbOrder
        {
            ClientId = client.Id,
            OrderedAt = orderDate
        });

        var orderItemId = await _api.CreateDbOrderItem(new DbOrderItem
        {
            OrderId = orderId,
            ProductId = productId,
            Price = product1Price,
            Qty = orderItemQty,
            Comment = orderItemComment
        });
        
        var (status, _) = await _api.Client.DoDelete(token, $"/orders/{orderId}/items/123");
        Assert.Equal(HttpStatusCode.NotFound, status);
    }
}