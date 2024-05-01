using TechStore.Domain.Models.Products;

namespace TechStore.Domain.Services;

public interface IProductsDiscountsService
{
    void Start();

    void Stop();

    event Action<Product, double> OnDiscount;
}