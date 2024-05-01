using System;
using System.Net;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using TechStore.DB.SQLite.Entities.Orders;
using TechStore.Tests.Extensions;

using Xunit;

namespace TechStore.Tests.Orders;

[Collection(nameof(TechStoreApiTests))]
public class CreateOrderReviewTests : IClassFixture<TechStoreApiFixture>
{
    private readonly TechStoreApiFixture _api;

    public CreateOrderReviewTests(TechStoreApiFixture api)
    {
        _api = api;
    }

    [Theory, AutoData]
    public async Task CreateOrderReview_Must_Create_Order_Review(
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
        
        var (status, _) = await _api.Client.DoPost(token, $"/orders/{orderId}/review", new
        {
            Rate = rate,
            Comment = comment
        });

        Assert.Equal(HttpStatusCode.OK, status);

        var review = Assert.Single(await _api.GetDbOrderReviews());
        
        Assert.Equal(rate, review.Rate);
        Assert.Equal(comment, review.Comment);
    }

    [Theory, AutoData]
    public async Task CreateOrderReview_Must_Return_Not_Found_When_Order_Not_Exists(
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
        
        var (status, _) = await _api.Client.DoPost(token, "/orders/123/review", new
        {
            Rate = rate,
            Comment = comment
        });

        Assert.Equal(HttpStatusCode.NotFound, status);

        Assert.Empty(await _api.GetDbOrderReviews());
    }
    
    [Theory, AutoData]
    public async Task CreateOrderReview_Must_Return_Bad_Request_When_Order_Has_Review(
        int rate1,
        int rate2,
        string comment1,
        string comment2)
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
            Rate = rate1,
            Comment = comment1
        });
        
        var (status, _) = await _api.Client.DoPost(token, $"/orders/{orderId}/review", new
        {
            Rate = rate2,
            Comment = comment2
        });

        Assert.Equal(HttpStatusCode.BadRequest, status);

        var review = Assert.Single(await _api.GetDbOrderReviews());
        
        Assert.Equal(rate1, review.Rate);
        Assert.Equal(comment1, review.Comment);
    }
}