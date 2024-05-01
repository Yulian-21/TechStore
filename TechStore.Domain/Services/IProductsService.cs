using TechStore.Domain.Models.Products;

namespace TechStore.Domain.Services;

public interface IProductsService
{
    bool ProductExists(int productId);
    
    Product GetProductById(int productId);

    Product CreateProduct(Product product);
    
    Product UpdateProduct(Product product);

    IEnumerable<Product> GetAllProducts();

    void DeleteProduct(int productId);
    
    (string ContentType, string Name, Stream Content) GetProductImage(int productId, int imageId);

    (string ContentType, string Name, Stream Content) GetProductImageById(int imageId);

    (string ContentType, string Name, Stream Content)  GetProductDocument(int productId, int documentId);

    public string GetAndDeleteProductCloudImage(int productId, int resourceId);

    ProductResource AddProductImage(int productId, string contentType, string name, Stream content);
    
    ProductResource AddProductDocument(int productId, string contentType, string name, Stream content);

    void DeleteProductImage(int productId, int imageId);
    
    void DeleteProductDocument(int productId, int documentId);
}