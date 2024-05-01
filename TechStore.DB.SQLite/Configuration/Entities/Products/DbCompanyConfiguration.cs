using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TechStore.DB.SQLite.Entities.Products;

namespace TechStore.DB.SQLite.Configuration.Entities.Products;

public class DbCompanyConfiguration : IEntityTypeConfiguration<DbCompany>
{
    public void Configure(EntityTypeBuilder<DbCompany> builder)
    {
        builder.ToTable("Companies");
        
        builder.HasKey(e => e.Id);

        builder.Property(x => x.Name)
            .HasColumnName("Name")
            .HasMaxLength(200);
    }
}