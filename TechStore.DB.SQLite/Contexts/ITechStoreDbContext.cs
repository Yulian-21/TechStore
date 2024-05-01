using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using TechStore.DB.Services;
using TechStore.DB.SQLite.Entities;
using TechStore.DB.SQLite.Entities.Orders;
using TechStore.DB.SQLite.Entities.Products;
using TechStore.DB.SQLite.Entities.Users;

namespace TechStore.DB.SQLite.Contexts;

public interface ITechStoreDbContext : IDbMigrationService
{
    DbSet<DbAdmin> Admins { get; }
    DbSet<DbClient> Clients { get; }
    
    DbSet<DbCompany> Companies { get; }
    
    DbSet<DbProduct> Products { get; }
    DbSet<DbProductResource> ProductResources { get; }
    
    DbSet<DbOrder> Orders { get; }
    DbSet<DbOrderItem> OrderItems { get; }
    DbSet<DbOrderReview> OrderReviews { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}