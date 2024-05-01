using System.Diagnostics.CodeAnalysis;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TechStore.DB.SQLite.Entities.Orders;

namespace TechStore.DB.SQLite.Configuration.Entities.Orders;

[ExcludeFromCodeCoverage]
public class DbOrderReviewConfiguration : IEntityTypeConfiguration<DbOrderReview>
{
    public void Configure(EntityTypeBuilder<DbOrderReview> builder)
    {
        builder.ToTable("OrderReviews");
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.OrderId)
            .HasColumnName("OrderId")
            .IsRequired();
        
        builder.Property(e => e.Rate)
            .HasColumnName("Rate")
            .IsRequired();
        
        builder.Property(e => e.Comment)
            .HasColumnName("Comment")
            .HasMaxLength(200);

        builder.HasOne(d => d.Order)
            .WithMany(p => p.Reviews)
            .HasForeignKey(d => d.OrderId);
    }
}
