using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TechStore.DB.SQLite.Entities.Products;

namespace TechStore.DB.SQLite.Configuration.Entities.Products;

public class DbProductConfiguration : IEntityTypeConfiguration<DbProduct>
{
    public void Configure(EntityTypeBuilder<DbProduct> builder)
    {
        builder.ToTable("Products");
        
        builder.HasKey(e => e.Id);

        builder.Property(x => x.Name)
            .HasColumnName("Name")
            .IsRequired()
            .HasMaxLength(60);
        
        builder.Property(x => x.Description)
            .HasColumnName("Description")
            .HasMaxLength(1000);
        
        builder.Property(x => x.Model)
            .HasColumnName("Model")
            .HasMaxLength(20);
        
        builder.Property(x => x.Price)
            .HasColumnName("Price")
            .IsRequired();
        
        builder.Property(x => x.Available)
            .HasColumnName("Available");
        
        builder.Property(x => x.Available)
            .HasColumnName("Country")
            .HasMaxLength(60);
        
        builder.HasOne(d => d.Supplier)
            .WithMany(p => p.Products)
            .HasForeignKey(d => d.SupplierId);
    }
}