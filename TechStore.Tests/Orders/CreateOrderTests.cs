using System;
using System.Net;
using System.Threading.Tasks;

using TechStore.Tests.Extensions;

using Xunit;

namespace TechStore.Tests.Orders;

[Collection(nameof(TechStoreApiTests))]
public class CreateOrderTests : IClassFixture<TechStoreApiFixture>
{
    private readonly TechStoreApiFixture _api;

    public CreateOrderTests(TechStoreApiFixture api)
    {
        _api = api;
    }

    [Fact]
    public async Task CreateOrder_Must_Create_Order_In_Db()
    {
        await _api.CleanupDb();
        
        var token = await _api.Login();
        var client = Assert.Single(await _api.GetDbClients());

        var (status, _) = await _api.Client.DoPost(token, "/orders/create", new { });
        Assert.Equal(HttpStatusCode.OK, status);

        var order = Assert.Single(await _api.GetDbOrders());
        
        Assert.Equal(client.Id, order.ClientId);
        
        Assert.True((DateTime.UtcNow - order.OrderedAt).TotalSeconds <= 3);
        Assert.Null(order.CompletedAt);
    }
}