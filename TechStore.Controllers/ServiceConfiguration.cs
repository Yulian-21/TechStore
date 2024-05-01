using TechStore.Controllers.Notifications;

namespace TechStore.Controllers;

public static class ServiceConfiguration
{
    public static IServiceCollection AddTechStoreControllers(
        this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ModelsMapper));
        
        services.AddControllers();
        services.AddHostedService<ProductsDiscountsNotificationService>();
        
        return services;
    }
}