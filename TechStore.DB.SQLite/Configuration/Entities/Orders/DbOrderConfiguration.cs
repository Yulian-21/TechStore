using System.Diagnostics.CodeAnalysis;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TechStore.DB.SQLite.Entities.Orders;

namespace TechStore.DB.SQLite.Configuration.Entities.Orders;

[ExcludeFromCodeCoverage]
public class DbOrderConfiguration : IEntityTypeConfiguration<DbOrder>
{
    public void Configure(EntityTypeBuilder<DbOrder> builder)
    {
        builder.ToTable("Orders");
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.ClientId)
            .HasColumnName("ClientId")
            .IsRequired();
        
        builder.Property(e => e.OrderedAt)
            .HasColumnName("OrderedAt")
            .IsRequired();
        
        builder.Property(e => e.CompletedAt)
            .HasColumnName("ReceivedAt");
        
        builder.HasOne(d => d.Client)
            .WithMany(p => p.Orders)
            .HasForeignKey(d => d.ClientId);
    }
}