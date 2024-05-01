using System;
using System.Net;
using System.Threading.Tasks;

using TechStore.DB.SQLite.Entities.Orders;
using TechStore.Tests.Extensions;

using Xunit;

namespace TechStore.Tests.Orders;

[Collection(nameof(TechStoreApiTests))]
public class UpdateOrderAsCompletedTests : IClassFixture<TechStoreApiFixture>
{
    private readonly TechStoreApiFixture _api;

    public UpdateOrderAsCompletedTests(TechStoreApiFixture api)
    {
        _api = api;
    }
    
    [Fact]
    public async Task UpdateOrderAsCompleted_Must_Return_Not_Found_When_Order_Not_Exists()
    {
        await _api.CleanupDb();
        
        var token = await _api.LoginAdmin();
        var (status, _) = await _api.Client.DoPut(token, $"/orders/{123}/completed", new {});
        
        Assert.Equal(HttpStatusCode.NotFound, status);
    }
}