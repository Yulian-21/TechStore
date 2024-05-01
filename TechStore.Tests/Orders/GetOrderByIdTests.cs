using System;
using System.Net;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using TechStore.DB.SQLite.Entities.Orders;
using TechStore.DB.SQLite.Entities.Products;
using TechStore.Tests.Extensions;
using TechStore.Tests.Orders.Models;

using Xunit;

namespace TechStore.Tests.Orders;

[Collection(nameof(TechStoreApiTests))]
public class GetOrderByIdTests : IClassFixture<TechStoreApiFixture>
{
    private readonly TechStoreApiFixture _api;

    public GetOrderByIdTests(TechStoreApiFixture api)
    {
        _api = api;
    }

    [Theory, AutoData]
    public async Task GetOrderById_Must_Return_Order_With_All_Details(
        double orderItemPrice,
        int orderItemQty,
        string orderItemComment,
        int orderReviewRate,
        string orderReviewComment)
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
            SupplierId = companyId
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
            ProductId = productId,
            Price = orderItemPrice,
            Qty = orderItemQty,
            Comment = orderItemComment
        });

        await _api.CreateDbOrderReview(new DbOrderReview
        {
            OrderId = orderId,
            Rate = orderReviewRate,
            Comment = orderReviewComment
        });
        
        var (status, order) = await _api.Client.DoGet<TestOrder>(token, $"/orders/{orderId}");
        Assert.Equal(HttpStatusCode.OK, status);
        
        Assert.Equal(client.Id, order.ClientId);
        Assert.Equal(orderDate, order.OrderedAt);

        var orderItem = Assert.Single(order.Items);
        Assert.Equal(productId, orderItem.ProductId);
        Assert.Equal(orderItemPrice, orderItem.Price);
        Assert.Equal(orderItemQty, orderItem.Qty);
        Assert.Equal(orderItemComment, orderItem.Comment);
        
        Assert.NotNull(order.Review);
        Assert.Equal(orderReviewRate, order.Review.Rate);
        Assert.Equal(orderReviewComment, order.Review.Comment);
    }
    
    [Fact]
    public async Task GetOrderById_Must_Return_Not_Found_When_Order_Not_Exists()
    {
        await _api.CleanupDb();
        
        var token = await _api.Login();
        
        var (status, _) = await _api.Client.DoGet(token, "/orders/123");
        Assert.Equal(HttpStatusCode.NotFound, status);
    }
}