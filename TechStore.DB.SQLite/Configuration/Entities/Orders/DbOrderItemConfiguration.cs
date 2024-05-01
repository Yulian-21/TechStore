using System.Diagnostics.CodeAnalysis;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TechStore.DB.SQLite.Entities.Orders;

namespace TechStore.DB.SQLite.Configuration.Entities.Orders;

[ExcludeFromCodeCoverage]
public class DbOrderItemConfiguration : IEntityTypeConfiguration<DbOrderItem>
{
    public void Configure(EntityTypeBuilder<DbOrderItem> builder)
    {
        builder.ToTable("OrderItems");
        
        builder.HasKey(e => e.Id);

        builder.Property(e => e.OrderId)
            .HasColumnName("OrderId")
            .IsRequired();
        
        builder.Property(e => e.ProductId)
            .HasColumnName("ProductId")
            .IsRequired();
        
        builder.Property(e => e.Qty)
            .HasColumnName("Qty")
            .IsRequired();
        
        builder.Property(e => e.Price)
            .HasColumnName("Price")
            .IsRequired();
        
        builder.Property(e => e.Comment)
            .HasMaxLength(200);
        
        builder.HasOne(d => d.Order)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(d => d.OrderId);
        
        builder.HasOne(d => d.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(d => d.ProductId);
    }
}