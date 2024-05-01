using System;
using System.Net;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using TechStore.DB.SQLite.Entities.Orders;
using TechStore.Tests.Extensions;

using Xunit;

namespace TechStore.Tests.Orders;

[Collection(nameof(TechStoreApiTests))]
public class DeleteOrderReviewTests : IClassFixture<TechStoreApiFixture>
{
    private readonly TechStoreApiFixture _api;

    public DeleteOrderReviewTests(TechStoreApiFixture api)
    {
        _api = api;
    }

    [Theory, AutoData]
    public async Task DeleteOrderReview_Must_Delete_Order_Review(
        int rate,
        string comment)
    {
        await _api.CleanupDb();
        
        var token = await _api.Login();
        var client = Assert.Single(await _api.GetDbClients());
        
        var orderId = await _api.CreateDbOrder(new DbOrder
        {
            ClientId = client.Id,
            OrderedAt = DateTime.UtcNow
        });
        
        await _api.CreateDbOrderReview(new DbOrderReview
        {
            OrderId = orderId,
            Rate = rate,
            Comment = comment
        });
        
        var (status, _) = await _api.Client.DoDelete(token, $"/orders/{orderId}/review");
        Assert.Equal(HttpStatusCode.NoContent, status);
        
        Assert.Empty(await _api.GetDbOrderReviews());
    }

    [Theory, AutoData]
    public async Task DeleteOrderReview_Must_Return_Not_Found_When_Order_Not_Exists(
        int rate,
        string comment)
    {
        await _api.CleanupDb();
        
        var token = await _api.Login();
        var client = Assert.Single(await _api.GetDbClients());
        
        var orderId = await _api.CreateDbOrder(new DbOrder
        {
            ClientId = client.Id,
            OrderedAt = DateTime.UtcNow
        });
        
        await _api.CreateDbOrderReview(new DbOrderReview
        {
            OrderId = orderId,
            Rate = rate,
            Comment = comment
        });
        
        var (status, _) = await _api.Client.DoDelete(token, "/orders/123/review");
        Assert.Equal(HttpStatusCode.NotFound, status);
    }

    [Fact]
    public async Task DeleteOrderReview_Must_Return_Not_Found_When_Review_Not_Exists()
    {
        await _api.CleanupDb();
        
        var token = await _api.Login();
        var client = Assert.Single(await _api.GetDbClients());
        
        var orderId = await _api.CreateDbOrder(new DbOrder
        {
            ClientId = client.Id,
            OrderedAt = DateTime.UtcNow
        });
        
        var (status, _) = await _api.Client.DoDelete(token, $"/orders/{orderId}/review");
        Assert.Equal(HttpStatusCode.NotFound, status);
    }
}