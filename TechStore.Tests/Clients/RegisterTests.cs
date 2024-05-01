using System.Net;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using TechStore.DB.SQLite.Entities.Users;
using TechStore.Tests.Extensions;

using Xunit;

namespace TechStore.Tests.Clients;

[Collection(nameof(TechStoreApiTests))]
public class RegisterTests : IClassFixture<TechStoreApiFixture>
{
    private readonly TechStoreApiFixture _api;

    public RegisterTests(TechStoreApiFixture api)
    {
        _api = api;
    }

    [Theory, AutoData]
    public async Task Register_Must_Create_New_Client(
        string firstName,
        string lastName,
        string email,
        string password)
    {
        await _api.CleanupDb();

        var (status, _) = await _api.Client.DoPost(null, "/clients/register", new
        {
            FirstName = firstName,
            LastName = lastName,

            Email = email,
            Password = password
        });

        Assert.Equal(HttpStatusCode.OK, status);

        var client = Assert.Single(await _api.GetDbClients());
        
        Assert.Equal(firstName, client.FirstName);
        Assert.Equal(lastName, client.LastName);
        
        Assert.Equal(email, client.Email);
    }
    
    [Theory, AutoData]
    public async Task Register_Must_Return_Bad_Request_When_Email_Duplicated(
        string firstName,
        string lastName,
        string email,
        string password)
    {
        await _api.CleanupDb();

        await _api.CreateDbClient(new DbClient
        {
            FirstName = "FirstName",
            LastName = "LastName",

            Email = email,
            Password = password
        });
        
        var (status, _) = await _api.Client.DoPost(null, "/clients/register", new
        {
            FirstName = firstName,
            LastName = lastName,

            Email = email,
            Password = password
        });

        Assert.Equal(HttpStatusCode.BadRequest, status);
    }
}