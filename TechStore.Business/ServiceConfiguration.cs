using System;
using System.Text;

using FluentValidation;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using TechStore.Business.Services;
using TechStore.Business.Services.Authentication;
using TechStore.Business.Services.Content;

using TechStore.Domain.Models.Products;
using TechStore.Domain.Services;
using TechStore.Domain.Services.Authentication;

namespace TechStore.Business;

public static class ServiceConfiguration
{
    public static IServiceCollection AddTechStoreJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var authenticationConfiguration = new JwtAuthenticationConfiguration(configuration);
        
        services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(authenticationConfiguration.Secret),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };
            });
        
        return services
            .AddTransient<IJwtAuthenticationConfiguration, JwtAuthenticationConfiguration>()
            .AddTransient<IAuthenticationService, JwtAuthenticationService>();
    }
    
    public static IServiceCollection AddTechStoreBusiness(this IServiceCollection services)
    {
        return services
            .AddTransient<IDiskContentStorageConfiguration, DiskContentStorageConfiguration>()
            .AddTransient<IContentStorageService, DiskContentStorageService>()
            
            .AddTransient<IClientService, ClientsService>()

            .AddTransient<IAdminsService, AdminsService>()

            .AddTransient<ICompaniesService, CompaniesService>()
            
            .AddTransient<IProductsService, ProductsService>()

            .AddSingleton<IProductsDiscountsService, ProductsDiscountsService>()

            .AddTransient<IOrdersService, OrdersService>()
            
            .AddTransient<IOrderItemsService, OrderItemsService>()

            .AddTransient<IOrderReviewsService, OrderReviewsService>();
    }
}