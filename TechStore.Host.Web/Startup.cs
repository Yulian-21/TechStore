using System.IO;
using System.IO.Abstractions;
using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using TechStore.Business;
using TechStore.Controllers;
using TechStore.Controllers.Notifications.FrontendChannel;
using TechStore.DB.Configuration;
using TechStore.DB.Services;
using TechStore.DB.SQLite;

using SQLiteDbConfiguration = TechStore.DB.SQLite.Configuration.DbConfiguration;

namespace TechStore.Host.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IFileSystem, FileSystem>();
            
            var healthChecks = services.AddHealthChecks();

            var persistenceConfiguration = Configuration.Get<DbConfiguration>();
            switch (persistenceConfiguration?.DbType)
            {
                case DbType.SQLite:

                    var projectDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", ".."));
                    var databasePath = Path.Combine(projectDirectory, ".database", "TechStore-empty.db");

                    var sqliteDbConfiguration = Configuration.GetSection("SQLite").Get<SQLiteDbConfiguration>();
                    if (sqliteDbConfiguration is null) throw new Exception("Unknown configuration for SQLite Db.");
                    sqliteDbConfiguration.RelativePath = databasePath;

                    sqliteDbConfiguration.DbType = persistenceConfiguration.DbType;
                    sqliteDbConfiguration.UseInMemoryDb = persistenceConfiguration.UseInMemoryDb;
        
                    services.AddTechStorePersistenceSQLite(sqliteDbConfiguration, healthChecks);
                    break;
    
                default:
                    throw new Exception($"Unknown Db Type: {persistenceConfiguration?.DbType}");
            }

            services.AddTechStoreJwtAuthentication(Configuration);
            services.AddTechStoreBusiness();
            services.AddTechStoreControllers();

            services.AddSignalR();
            
            services.AddEndpointsApiExplorer();
            AddAppSwaggerGenWithJwt(services, "Tech Store API");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (Configuration.GetValue<bool>("UseInMemoryDb") == false)
            {
                using var scope = app.ApplicationServices.CreateScope();
                var databaseMigrationService = scope.ServiceProvider.GetRequiredService<IDbMigrationService>();
                databaseMigrationService.Migrate();
            }

            app.UseRouting();
            app.UseStaticFiles();
            
            app.UseCors(_ => _
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader().WithOrigins("http://localhost:3000"));

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllers();
                endpoints.MapHub<FrontendChannelHub>("/notifications");
            });
            
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        private static void AddAppSwaggerGenWithJwt(IServiceCollection services, string title)
        {
            const string bearer = "Bearer";
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = title, Version = "v1" });

                c.AddSecurityDefinition(
                    bearer,
                    new OpenApiSecurityScheme
                    {
                        Description =
                            "JWT Authorization header using the Bearer scheme.\r\n Enter 'Bearer' [space] and then your token in the text input below.",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = bearer,
                    });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = bearer,
                            },
                            Scheme = "oauth2",
                            Name = bearer,
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    },
                });
            });
        }
    }
}