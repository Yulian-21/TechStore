using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using TechStore.DB.SQLite.Entities.Orders;
using TechStore.DB.SQLite.Entities.Users;

using TechStore.Tests.Extensions;
using TechStore.Tests.Orders.Models;

using Xunit;

namespace TechStore.Tests.Orders;

[Collection(nameof(TechStoreApiTests))]
public class GetClientOrdersTests : IClassFixture<TechStoreApiFixture>
{
    private readonly TechStoreApiFixture _api;

    public GetClientOrdersTests(TechStoreApiFixture api)
    {
        _api = api;
    }

    [Fact]
    public async Task GetClientOrders_Must_Return_Client_Orders()
    {
        await _api.CleanupDb();
        
        var token = await _api.Login();
        var client = Assert.Single(await _api.GetDbClients());

        var client2Id = await _api.CreateDbClient(new DbClient
        {
            FirstName = "Name",
            LastName = "Name",
            
            Email = "Email",
            Password = "Password"
        });

        var orderDate = DateTime.UtcNow;

        var orderId1 = await _api.CreateDbOrder(new DbOrder
        {
            ClientId = client.Id,
            OrderedAt = orderDate
        });
        
        var orderId2 = await _api.CreateDbOrder(new DbOrder
        {
            ClientId = client.Id,
            OrderedAt = orderDate
        });
        
        await _api.CreateDbOrder(new DbOrder
        {
            ClientId = client2Id,
            OrderedAt = orderDate
        });
        
        var (status, orders) = await _api.Client.DoGet<List<TestOrder>>(token, $"/orders/clients/{client.Id}");
        Assert.Equal(HttpStatusCode.OK, status);
        
        Assert.Equal(2, orders.Count);

        Assert.Contains(orders, x => x.Id == orderId1);
        Assert.Contains(orders, x => x.Id == orderId2);
    }
    
    [Fact]
    public async Task GetClientOrders_Must_Return_Not_Found_When_Client_Not_Exists()
    {
        await _api.CleanupDb();
        
        var token = await _api.Login();
        var client = Assert.Single(await _api.GetDbClients());
        
        var (status, _) = await _api.Client.DoGet(token, "/orders/clients/123");
        Assert.Equal(HttpStatusCode.NotFound, status);
    }
}