using System;
using System.Linq;
using System.Threading;

using Microsoft.Extensions.DependencyInjection;

using TechStore.DB.Repositories;
using TechStore.Domain.Models.Products;
using TechStore.Domain.Services;

namespace TechStore.Business.Services;

public class ProductsDiscountsService : IProductsDiscountsService
{
    private readonly IServiceProvider _serviceProvider;
    private Thread _discountsGeneratorThread;

    public ProductsDiscountsService(
        IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        _serviceProvider = serviceProvider;
    }
    
    public void Start()
    {
        _discountsGeneratorThread = new Thread(DiscountsGeneratorMethod);
        _discountsGeneratorThread.IsBackground = true;
        
        _discountsGeneratorThread.Start();
    }

    public void Stop()
    {
    }

    public event Action<Product, double> OnDiscount;

    private void DiscountsGeneratorMethod()
    {
        while (true)
        {
            Thread.Sleep(10000);
            
            using var scope = _serviceProvider.CreateScope();
            var productsRepository = scope.ServiceProvider.GetService<IProductsRepository>();

            var products = productsRepository.GetAll().ToList();
            if (!products.Any()) continue;
            
            var productIndex = new Random().Next(products.Count);

            var product = products[productIndex];
            var discount = new Random().Next(10, 26);

            OnDiscount?.Invoke(product, discount);
        }
    }
}