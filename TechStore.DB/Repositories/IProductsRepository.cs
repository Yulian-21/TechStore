using TechStore.Domain.Models.Products;

namespace TechStore.DB.Repositories;

public interface IProductsRepository : IDataRepository<Product>
{
    bool ProductExists(int productId);
}