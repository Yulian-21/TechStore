using System.Collections.Generic;

using TechStore.Domain.Models.Products;

namespace TechStore.DB.Repositories;

public interface IProductResourcesRepository : IDataRepository<ProductResource>
{
    List<ProductResource> GetProductResources(int productId);
}