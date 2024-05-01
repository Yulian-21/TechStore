using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TechStore.DB.SQLite.Entities.Users;

namespace TechStore.DB.SQLite.Configuration.Entities.Users;

public class DbClientShippingAddressConfiguration : IEntityTypeConfiguration<DbClientShippingAddress>
{
    public void Configure(EntityTypeBuilder<DbClientShippingAddress> builder)
    {
        builder.ToTable("ClientShippingAddresses");
        
        builder.HasKey(e => e.Id);

        builder.Property(x => x.Address)
            .HasColumnName("Address")
            .IsRequired()
            .HasMaxLength(200);
        
        builder.HasOne(d => d.Client)
            .WithMany(p => p.ShippingAddresses)
            .HasForeignKey(d => d.ClientId);
    }
}