using System;
using System.Net;
using System.Threading.Tasks;

using TechStore.DB.SQLite.Entities.Orders;
using TechStore.Tests.Extensions;

using Xunit;

namespace TechStore.Tests.Orders;

[Collection(nameof(TechStoreApiTests))]
public class DeleteOrderTests : IClassFixture<TechStoreApiFixture>
{
    private readonly TechStoreApiFixture _api;

    public DeleteOrderTests(TechStoreApiFixture api)
    {
        _api = api;
    }
    
    [Fact]
    public async Task DeleteOrder_Must_Delete_Order_From_Db()
    {
        await _api.CleanupDb();
        
        var token = await _api.Login();
        var client = Assert.Single(await _api.GetDbClients());
        
        var orderId = await _api.CreateDbOrder(new DbOrder
        {
            ClientId = client.Id,
            OrderedAt = DateTime.UtcNow
        });

        var (status, _) = await _api.Client.DoDelete(token, $"/orders/{orderId}");

        Assert.Equal(HttpStatusCode.NoContent, status);
        Assert.Empty(await _api.GetDbOrders());
    }

    [Fact]
    public async Task DeleteOrder_Must_Return_Not_Found_When_Order_Not_Exists()
    {
        await _api.CleanupDb();

        var token = await _api.Login();
        var (status, _) = await _api.Client.DoDelete(token, "/orders/123");

        Assert.Equal(HttpStatusCode.NotFound, status);
    }
}