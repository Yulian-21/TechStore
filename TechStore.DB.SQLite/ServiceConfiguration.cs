using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using TechStore.DB.Repositories;
using TechStore.DB.Services;

using TechStore.DB.SQLite.Configuration;
using TechStore.DB.SQLite.Contexts;
using TechStore.DB.SQLite.Repositories;

using TechStore.Domain.Models.Products;
using TechStore.Domain.Models.Users;

namespace TechStore.DB.SQLite;

public static class ServiceConfiguration
{
    public static IServiceCollection AddTechStorePersistenceSQLite(
        this IServiceCollection services,
        DbConfiguration configuration,
        IHealthChecksBuilder healthChecksBuilder)
    {
        services.AddAutoMapper(typeof(ModelsMapper));
        
        if (configuration.UseInMemoryDb)
        {
            services.AddDbContext<TechStoreDbContext>(_ => _.UseInMemoryDatabase("InMemoryTechStoreDb"));
        }
        else
        {
           // builder.Services.AddDbContext<DbShopContext>(db => db.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));//"Data Source=..\DAL\DB\Shop.db" 
            var connectionString = configuration.GetConnectionString();
            services.AddDbContext<TechStoreDbContext>(db => db.UseSqlite(connectionString));
        }
        
        services.AddTransient<ITechStoreDbContext>(_ =>
            _.GetRequiredService<TechStoreDbContext>());
        
        services.AddTransient<IDbMigrationService>(_ =>
            _.GetRequiredService<TechStoreDbContext>());
        
        healthChecksBuilder?.
            AddDbContextCheck<TechStoreDbContext>();

        return services
            .AddTransient<IAdminsRepository, AdminRepository>()
            .AddTransient<IClientsRepository, ClientsRepository>()
            
            .AddTransient<ICompaniesRepository, CompaniesRepository>()
            
            .AddTransient<IProductsRepository, ProductsRepository>()
            .AddTransient<IProductResourcesRepository, ProductResourcesRepository>()

            .AddTransient<IOrdersRepository, OrdersRepository>()
            .AddTransient<IOrderItemsRepository, OrderItemsRepository>()
            .AddTransient<IOrderReviewsRepository, OrderReviewsRepository>();
    }
}
