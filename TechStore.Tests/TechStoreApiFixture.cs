using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;

using TechStore.DB.SQLite.Contexts;
using TechStore.DB.SQLite.Entities.Orders;
using TechStore.DB.SQLite.Entities.Products;
using TechStore.DB.SQLite.Entities.Users;

using TechStore.Host.Web;

using TechStore.Tests.Authentication.Models;
using TechStore.Tests.Extensions;

using Xunit;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Json;

namespace TechStore.Tests
{
    public class TechStoreApiFixture : WebApplicationFactory<Startup>
    {
        public HttpClient Client { get; }

        public TechStoreApiFixture()
        {
            Client = CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("http://localhost"),
                AllowAutoRedirect = false
            });
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((hostingContext, configuration) =>
            {
                configuration.AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["DbType"] = "SQLite",
                    ["UseInMemoryDb"] = "true"
                });
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Client?.Dispose();
            }
            base.Dispose(disposing);
        }

        public async Task CleanupDb()
        {
            using var scope = Services.CreateScope();
            var databaseContext = scope.ServiceProvider.GetRequiredService<ITechStoreDbContext>();

            databaseContext.OrderReviews.RemoveRange(databaseContext.OrderReviews);
            databaseContext.OrderItems.RemoveRange(databaseContext.OrderItems);
            databaseContext.Orders.RemoveRange(databaseContext.Orders);
            databaseContext.ProductResources.RemoveRange(databaseContext.ProductResources);
            databaseContext.Products.RemoveRange(databaseContext.Products);
            databaseContext.Companies.RemoveRange(databaseContext.Companies);
            databaseContext.Clients.RemoveRange(databaseContext.Clients);
            databaseContext.Admins.RemoveRange(databaseContext.Admins);

            await databaseContext.SaveChangesAsync();
        } 

        public async Task<string> RegisterClient(string firstName, string lastName, string email, string password)
        {
            var registerRequest = new
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password
            };

            var content = new StringContent(JsonConvert.SerializeObject(registerRequest), Encoding.UTF8, "application/json");

            var response = await Client.PostAsync("/clients/register", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            return email;
        }

        public async Task<string> RegisterAdmin(string firstName, string lastName, string email, string password, string userName)
        {
            var registerRequest = new
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password,
                UserName = userName
            };

            var content = new StringContent(JsonConvert.SerializeObject(registerRequest), Encoding.UTF8, "application/json");

            var response = await Client.PostAsync("/admins/register", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            return email;
        }

        public async Task<string> Login()
        {
            const string email = "TestsUser@gmail.com";
            const string password = "TestsUserPassword";

            await RegisterClient("TestsUser", "TestsUser", email, password);

            var loginRequest = new
            {
                Username = email,
                Password = password
            };

            var content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");

            var response = await Client.PostAsync("/authentication/login/client", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var authenticationResult = await response.Content.ReadFromJsonAsync<TestAuthenticationResult>();
            return authenticationResult.Token;
        }

        public async Task<string> LoginAdmin()
        {
            const string email = "TestsAdmin@gmail.com";
            const string password = "TestsAdminPassword";
            const string userName = "TestsAdmin";

            await RegisterAdmin("TestsAdmin", "TestsAdmin", email, password, userName);

            var loginRequest = new
            {
                Username = email,
                Password = password
            };

            var content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");

            var response = await Client.PostAsync("/authentication/login/admin", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var authenticationResult = await response.Content.ReadFromJsonAsync<TestAuthenticationResult>();
            return authenticationResult.Token;
        }

        public async Task<List<DbOrder>> GetDbOrders()
        {
            using var scope = Services.CreateScope();
            var databaseContext = scope.ServiceProvider.GetRequiredService<ITechStoreDbContext>();

            return await databaseContext.Orders.AsNoTracking().ToListAsync();
        }

        public async Task<int> CreateDbOrder(DbOrder order)
        {
            using var scope = Services.CreateScope();
            var databaseContext = scope.ServiceProvider.GetRequiredService<ITechStoreDbContext>();

            databaseContext.Orders.Add(order);
            await databaseContext.SaveChangesAsync();

            return order.Id;
        }

        public async Task<List<DbOrderItem>> GetDbOrderItems()
        {
            using var scope = Services.CreateScope();
            var databaseContext = scope.ServiceProvider.GetRequiredService<ITechStoreDbContext>();

            return await databaseContext.OrderItems.AsNoTracking().ToListAsync();
        }

        public async Task<int> CreateDbOrderItem(DbOrderItem orderItem)
        {
            using var scope = Services.CreateScope();
            var databaseContext = scope.ServiceProvider.GetRequiredService<ITechStoreDbContext>();

            databaseContext.OrderItems.Add(orderItem);
            await databaseContext.SaveChangesAsync();

            return orderItem.Id;
        }

        public async Task<List<DbOrderReview>> GetDbOrderReviews()
        {
            using var scope = Services.CreateScope();
            var databaseContext = scope.ServiceProvider.GetRequiredService<ITechStoreDbContext>();

            return await databaseContext.OrderReviews.AsNoTracking().ToListAsync();
        }

        public async Task CreateDbOrderReview(DbOrderReview orderReview)
        {
            using var scope = Services.CreateScope();
            var databaseContext = scope.ServiceProvider.GetRequiredService<ITechStoreDbContext>();

            databaseContext.OrderReviews.Add(orderReview);
            await databaseContext.SaveChangesAsync();
        }

        public async Task<List<DbProduct>> GetDbProducts()
        {
            using var scope = Services.CreateScope();
            var databaseContext = scope.ServiceProvider.GetRequiredService<ITechStoreDbContext>();

            return await databaseContext.Products.AsNoTracking().ToListAsync();
        }

        public async Task<int> CreateDbProduct(DbProduct product)
        {
            using var scope = Services.CreateScope();
            var databaseContext = scope.ServiceProvider.GetRequiredService<ITechStoreDbContext>();

            databaseContext.Products.Add(product);
            await databaseContext.SaveChangesAsync();

            return product.Id;
        }

        public async Task<List<DbProductResource>> GetDbProductResources()
        {
            using var scope = Services.CreateScope();
            var databaseContext = scope.ServiceProvider.GetRequiredService<ITechStoreDbContext>();

            return await databaseContext.ProductResources.AsNoTracking().ToListAsync();
        }

        public async Task<int> CreateDbProductResource(DbProductResource productResource)
        {
            using var scope = Services.CreateScope();
            var databaseContext = scope.ServiceProvider.GetRequiredService<ITechStoreDbContext>();

            databaseContext.ProductResources.Add(productResource);
            await databaseContext.SaveChangesAsync();

            return productResource.Id;
        }

        public async Task<List<DbCompany>> GetDbCompanies()
        {
            using var scope = Services.CreateScope();
            var databaseContext = scope.ServiceProvider.GetRequiredService<ITechStoreDbContext>();

            return await databaseContext.Companies.AsNoTracking().ToListAsync();
        }

        public async Task<int> CreateDbCompany(DbCompany company)
        {
            using var scope = Services.CreateScope();
            var databaseContext = scope.ServiceProvider.GetRequiredService<ITechStoreDbContext>();

            databaseContext.Companies.Add(company);
            await databaseContext.SaveChangesAsync();

            return company.Id;
        }

        public async Task<List<DbClient>> GetDbClients()
        {
            using var scope = Services.CreateScope();
            var databaseContext = scope.ServiceProvider.GetRequiredService<ITechStoreDbContext>();

            return await databaseContext.Clients.AsNoTracking().ToListAsync();
        }

        public async Task<int> CreateDbClient(DbClient client)
        {
            using var scope = Services.CreateScope();
            var databaseContext = scope.ServiceProvider.GetRequiredService<ITechStoreDbContext>();

            databaseContext.Clients.Add(client);
            await databaseContext.SaveChangesAsync();

            return client.Id;
        }

        public async Task<string> LoginAsAdmin()
        {
            // Авторизація як адміністратор
            var adminToken = await GetAdminToken();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
            return adminToken;
        }

        public async Task<string> LoginAsClient()
        {
            // Авторизація як клієнт
            var clientToken = await GetClientToken();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", clientToken);
            return clientToken;
        }

        private async Task<string> GetAdminToken()
        {
            const string adminUsername = "admin";
            const string adminPassword = "adminpassword";

            var adminTokenRequest = new HttpRequestMessage(HttpMethod.Post, "/authentication/login");
            adminTokenRequest.Content = new StringContent(JsonConvert.SerializeObject(new
            {
                Username = adminUsername,
                Password = adminPassword
            }), Encoding.UTF8, "application/json");

            var adminTokenResponse = await Client.SendAsync(adminTokenRequest);
            var adminTokenContent = await adminTokenResponse.Content.ReadAsStringAsync();
            var adminToken = JsonConvert.DeserializeObject<TestAuthenticationResult>(adminTokenContent)?.Token;

            Assert.Equal(HttpStatusCode.OK, adminTokenResponse.StatusCode);
            Assert.NotNull(adminToken);

            return adminToken;
        }

        private async Task<string> GetClientToken()
        {
            const string clientUsername = "client";
            const string clientPassword = "clientpassword";

            var clientTokenRequest = new HttpRequestMessage(HttpMethod.Post, "/authentication/login");
            clientTokenRequest.Content = new StringContent(JsonConvert.SerializeObject(new
            {
                Username = clientUsername,
                Password = clientPassword
            }), Encoding.UTF8, "application/json");

            var clientTokenResponse = await Client.SendAsync(clientTokenRequest);
            var clientTokenContent = await clientTokenResponse.Content.ReadAsStringAsync();
            var clientToken = JsonConvert.DeserializeObject<TestAuthenticationResult>(clientTokenContent)?.Token;

            Assert.Equal(HttpStatusCode.OK, clientTokenResponse.StatusCode);
            Assert.NotNull(clientToken);

            return clientToken;
        }
    }
}
