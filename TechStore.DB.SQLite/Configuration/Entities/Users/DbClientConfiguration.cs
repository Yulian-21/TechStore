using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TechStore.DB.SQLite.Entities.Users;

namespace TechStore.DB.SQLite.Configuration.Entities.Users;

public class DbClientConfiguration : IEntityTypeConfiguration<DbClient>
{
    public void Configure(EntityTypeBuilder<DbClient> builder)
    {
        builder.ToTable("Clients");
        
        builder.HasKey(e => e.Id);

        builder.Property(x => x.FirstName)
            .HasColumnName("FirstName")
            .IsRequired()
            .HasMaxLength(60);
        
        builder.Property(x => x.LastName)
            .HasColumnName("LastName")
            .IsRequired()
            .HasMaxLength(60);
        
        builder.Property(x => x.Email)
            .HasColumnName("Email")
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(x => x.Password)
            .HasColumnName("Password")
            .IsRequired()
            .HasMaxLength(100);


    }
}