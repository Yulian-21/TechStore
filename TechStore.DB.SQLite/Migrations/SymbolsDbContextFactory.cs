using System.Diagnostics.CodeAnalysis;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using TechStore.DB.SQLite.Contexts;

namespace TechStore.DB.SQLite.Migrations;

/// <summary>
/// This is Db Context Factory class created to support EF Migrations generation.
/// See "https://docs.microsoft.com/en-us/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli" for details.
/// Run "dotnet ef migrations add MIGRATION_NAME" from project's root directory.
/// </summary>
[ExcludeFromCodeCoverage]
public class TechStoreDbContextFactory : IDesignTimeDbContextFactory<TechStoreDbContext>
{
    public TechStoreDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<TechStoreDbContext>();
        options.UseSqlite("DB\\migration.db");
        
        return new TechStoreDbContext(options.Options);
    }
}
