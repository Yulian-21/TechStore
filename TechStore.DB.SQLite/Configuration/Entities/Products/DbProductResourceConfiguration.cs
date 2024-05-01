using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TechStore.DB.SQLite.Entities.Products;

namespace TechStore.DB.SQLite.Configuration.Entities.Products;

public class DbProductResourceConfiguration : IEntityTypeConfiguration<DbProductResource>
{
    public void Configure(EntityTypeBuilder<DbProductResource> builder)
    {
        builder.ToTable("ProductResources");
        
        builder.HasKey(e => e.Id);

        builder.Property(x => x.ProductId)
            .HasColumnName("ProductId")
            .IsRequired();

        builder.Property(x => x.StorageKey)
            .HasColumnName("StorageKey")
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(x => x.ContentType)
            .HasColumnName("ContentType")
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(x => x.Name)
            .HasColumnName("Name")
            .IsRequired()
            .HasMaxLength(100);
        
        builder.HasOne(d => d.Product)
            .WithMany(p => p.Resources)
            .HasForeignKey(d => d.ProductId);
    }
}