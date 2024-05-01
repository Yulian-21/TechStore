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
public class UpdateOrderItemTests : IClassFixture<TechStoreApiFixture>
{
    private readonly TechStoreApiFixture _api;

    public UpdateOrderItemTests(TechStoreApiFixture api)
    {
        _api = api;
    }

    [Theory, AutoData]
    public async Task UpdateOrderItem_Must_Update_Order_Item(
        double product1Price,
        double product2Price,
        int orderItemQty1,
        int orderItemQty2,
        string orderItemComment1,
        string orderItemComment2)
    {
        await _api.CleanupDb();
        
        var token = await _api.Login();
        var client = Assert.Single(await _api.GetDbClients());

        var companyId = await _api.CreateDbCompany(new DbCompany
        {
            Name = "Company 1"
        });
        
        var product1Id = await _api.CreateDbProduct(new DbProduct
        {
            Name = "Product 1",
            SupplierId = companyId,
            Price = (decimal)product1Price
        });
        
        var product2Id = await _api.CreateDbProduct(new DbProduct
        {
            Name = "Product 2",
            SupplierId = companyId,
            Price = (decimal)product2Price
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
            ProductId = product1Id,
            Price = product1Price,
            Qty = orderItemQty1,
            Comment = orderItemComment1
        });
        
        var (status, _) = await _api.Client.DoPut(token, $"/orders/{orderId}/items/{orderItemId}", new
        {
            ProductId = product2Id,
            Qty = orderItemQty2,
            Comment = orderItemComment2
        });
        
        Assert.Equal(HttpStatusCode.OK, status);

        var orderItem = Assert.Single(await _api.GetDbOrderItems());
        
        Assert.Equal(product2Id, orderItem.ProductId);
        Assert.Equal(orderItemQty2, orderItem.Qty);
        Assert.Equal(product2Price, orderItem.Price);
        Assert.Equal(orderItemComment2, orderItem.Comment);
    }
    
    [Theory, AutoData]
    public async Task UpdateOrderItem_Must_Return_Not_Found_When_Order_Not_Exists(
        double product1Price,
        double product2Price,
        int orderItemQty1,
        int orderItemQty2,
        string orderItemComment1,
        string orderItemComment2)
    {
        await _api.CleanupDb();
        
        var token = await _api.Login();
        var client = Assert.Single(await _api.GetDbClients());

        var companyId = await _api.CreateDbCompany(new DbCompany
        {
            Name = "Company 1"
        });
        
        var product1Id = await _api.CreateDbProduct(new DbProduct
        {
            Name = "Product 1",
            SupplierId = companyId,
            Price = (decimal)product1Price
        });
        
        var product2Id = await _api.CreateDbProduct(new DbProduct
        {
            Name = "Product 2",
            SupplierId = companyId,
            Price = (decimal)product2Price
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
            ProductId = product1Id,
            Price = product1Price,
            Qty = orderItemQty1,
            Comment = orderItemComment1
        });
        
        var (status, _) = await _api.Client.DoPut(token, $"/orders/123/items/{orderItemId}", new
        {
            ProductId = product2Id,
            Qty = orderItemQty2,
            Comment = orderItemComment2
        });
        
        Assert.Equal(HttpStatusCode.NotFound, status);
    }
    
    [Theory, AutoData]
    public async Task UpdateOrderItem_Must_Return_Not_Found_When_Order_Item_Not_Exists(
        double product1Price,
        double product2Price,
        int orderItemQty1,
        int orderItemQty2,
        string orderItemComment1,
        string orderItemComment2)
    {
        await _api.CleanupDb();
        
        var token = await _api.Login();
        var client = Assert.Single(await _api.GetDbClients());

        var companyId = await _api.CreateDbCompany(new DbCompany
        {
            Name = "Company 1"
        });
        
        var product1Id = await _api.CreateDbProduct(new DbProduct
        {
            Name = "Product 1",
            SupplierId = companyId,
            Price = (decimal)product1Price
        });
        
        var product2Id = await _api.CreateDbProduct(new DbProduct
        {
            Name = "Product 2",
            SupplierId = companyId,
            Price = (decimal)product2Price
        });

        var orderDate = DateTime.UtcNow;

        var orderId = await _api.CreateDbOrder(new DbOrder
        {
            ClientId = client.Id,
            OrderedAt = orderDate
        });

        await _api.CreateDbOrderItem(new DbOrderItem
        {
            OrderId = orderId,
            ProductId = product1Id,
            Price = product1Price,
            Qty = orderItemQty1,
            Comment = orderItemComment1
        });
        
        var (status, _) = await _api.Client.DoPut(token, $"/orders/{orderId}/items/123", new
        {
            ProductId = product2Id,
            Qty = orderItemQty2,
            Comment = orderItemComment2
        });
        
        Assert.Equal(HttpStatusCode.NotFound, status);
    }
    
    [Theory, AutoData]
    public async Task UpdateOrderItem_Must_Return_Not_Found_When_Product_Not_Exists(
        double product1Price,
        int orderItemQty1,
        int orderItemQty2,
        string orderItemComment1,
        string orderItemComment2)
    {
        await _api.CleanupDb();
        
        var token = await _api.Login();
        var client = Assert.Single(await _api.GetDbClients());

        var companyId = await _api.CreateDbCompany(new DbCompany
        {
            Name = "Company 1"
        });
        
        var product1Id = await _api.CreateDbProduct(new DbProduct
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
            ProductId = product1Id,
            Price = product1Price,
            Qty = orderItemQty1,
            Comment = orderItemComment1
        });
        
        var (status, _) = await _api.Client.DoPut(token, $"/orders/{orderId}/items/{orderItemId}", new
        {
            ProductId = 123,
            Qty = orderItemQty2,
            Comment = orderItemComment2
        });
        
        Assert.Equal(HttpStatusCode.NotFound, status);
    }
}