using AutoMapper;

using Microsoft.AspNetCore.SignalR;

using TechStore.Controllers.Models.Products;
using TechStore.Controllers.Notifications.FrontendChannel;
using TechStore.Domain.Models.Products;
using TechStore.Domain.Services;

namespace TechStore.Controllers.Notifications;

public class ProductsDiscountsNotificationService : IHostedService
{
    private const string NotificationType = "product-discount";
    
    private readonly IProductsDiscountsService _productsDiscountsService;
    private readonly IHubContext<FrontendChannelHub, IFrontendChannel> _hub;
    private readonly IMapper _mapper;

    public ProductsDiscountsNotificationService(
        IProductsDiscountsService productsDiscountsService,
        IHubContext<FrontendChannelHub, IFrontendChannel> hub,
        IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(productsDiscountsService);
        _productsDiscountsService = productsDiscountsService;
        
        ArgumentNullException.ThrowIfNull(hub);
        _hub = hub;
        
        ArgumentNullException.ThrowIfNull(mapper);
        _mapper = mapper;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _productsDiscountsService.OnDiscount += DoOnProductDiscount;
        _productsDiscountsService.Start();
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _productsDiscountsService.Stop();
        _productsDiscountsService.OnDiscount -= DoOnProductDiscount;

        return Task.CompletedTask;
    }

    private void DoOnProductDiscount(Product product, double discount)
    {
        var task = _hub.Clients.All.Send(new FrontendChannelNotification
        {
            Type = NotificationType,
            Payload = new ApiProductDiscount
            {
                Product = _mapper.Map<ApiProduct>(product),
                Discount = discount
            }
        });

        task.Wait();
    }
}