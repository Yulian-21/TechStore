using Microsoft.EntityFrameworkCore;

using TechStore.DB.SQLite.Configuration.Entities.Orders;
using TechStore.DB.SQLite.Configuration.Entities.Products;
using TechStore.DB.SQLite.Configuration.Entities.Users;

using TechStore.DB.SQLite.Entities.Orders;
using TechStore.DB.SQLite.Entities.Products;
using TechStore.DB.SQLite.Entities.Users;

namespace TechStore.DB.SQLite.Contexts;

public class TechStoreDbContext : DbContext, ITechStoreDbContext
{
    public DbSet<DbAdmin> Admins { get; set; }
    public DbSet<DbClient> Clients { get; set; }
    
    public DbSet<DbCompany> Companies { get; set; }
    
    public DbSet<DbProduct> Products { get; set; }
    public DbSet<DbProductResource> ProductResources { get; set; }
    
    public DbSet<DbOrder> Orders { get; set; }
    public DbSet<DbOrderItem> OrderItems { get; set; }
    public DbSet<DbOrderReview> OrderReviews { get; set; }

    public TechStoreDbContext(DbContextOptions<TechStoreDbContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        ApplyConfiguration<
            DbAdmin,
            DbAdminConfiguration>(builder);
        
        ApplyConfiguration<
            DbClient,
            DbClientConfiguration>(builder);
        
        ApplyConfiguration<
            DbClientShippingAddress,
            DbClientShippingAddressConfiguration>(builder);
        
        ApplyConfiguration<
            DbCompany,
            DbCompanyConfiguration>(builder);
        
        ApplyConfiguration<
            DbProduct,
            DbProductConfiguration>(builder);
        
        ApplyConfiguration<
            DbProductResource,
            DbProductResourceConfiguration>(builder);
        
        ApplyConfiguration<
            DbOrder,
            DbOrderConfiguration>(builder);
        
        ApplyConfiguration<
            DbOrderItem,
            DbOrderItemConfiguration>(builder);
        
        ApplyConfiguration<
            DbOrderReview,
            DbOrderReviewConfiguration>(builder);
    }
    
    private void ApplyConfiguration<TEntity, TConfiguration>(ModelBuilder modelBuilder)
        where TEntity : class
        where TConfiguration : IEntityTypeConfiguration<TEntity>, new()
    {
        var configuration = new TConfiguration();
        modelBuilder.ApplyConfiguration(configuration);
    }

    public void Migrate()
    {
        Database.Migrate();
    }
}
