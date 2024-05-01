using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using FluentValidation;

using TechStore.Business.Exceptions;
using TechStore.DB.Repositories;
using TechStore.Domain.Models.Products;
using TechStore.Domain.Services;

namespace TechStore.Business.Services;

public class ProductsService : IProductsService
{
    private static readonly List<string> ImageMimeTypes = new() 
    {
        "image/png",
        "image/jpeg"
    };
    
    private static readonly List<string> DocumentMimeTypes = new() 
    {
        "application/pdf"
    };
    
    private readonly IProductsRepository _productsRepository;

    private readonly ICompaniesService _companiesService;

    private readonly IProductResourcesRepository _productResourcesRepository;
    private readonly IContentStorageService _contentStorageService;
    
    public ProductsService(
        ICompaniesService companiesService,
        IProductsRepository productsRepository,
        IProductResourcesRepository productResourcesRepository,
        IContentStorageService contentStorageService)
    {
        
        ArgumentNullException.ThrowIfNull(productsRepository);
        _productsRepository = productsRepository;
        
        ArgumentNullException.ThrowIfNull(companiesService);
        _companiesService = companiesService;
        
        ArgumentNullException.ThrowIfNull(productResourcesRepository);
        _productResourcesRepository = productResourcesRepository;
        
        ArgumentNullException.ThrowIfNull(contentStorageService);
        _contentStorageService = contentStorageService;
    }

    public bool ProductExists(int productId)
    {
        return _productsRepository.ProductExists(productId);
    }
    
    public Product GetProductById(int productId)
    {
        var product = _productsRepository.Get(productId);
        _ = product ?? throw new UnknownModelException("Product", "Id", productId);

        var resources = _productResourcesRepository.GetProductResources(productId);

        product.Images = resources.Where(ProductResourceIsImage).ToList(); 
        product.Documents = resources.Where(ProductResourceIsDocument).ToList(); 

        return product;
    }

    public IEnumerable<Product> GetAllProducts()
    {
        var products = _productsRepository.GetAll();
        _ = products ?? throw new UnknownModelException("Products", "Isn't", "Exist");

        foreach (var product in products)
        {
            var resources = _productResourcesRepository.GetProductResources(product.Id);

            product.Images = resources.Where(ProductResourceIsImage).ToList();
            product.Documents = resources.Where(ProductResourceIsDocument).ToList();
        }

        return products;
    }

    public Product CreateProduct(Product product)
    {
        var companyId = product.Supplier?.Id ?? 0;

        var companyExists = _companiesService.CompanyExists(companyId);
        if (!companyExists) throw new UnknownModelException("Company", "Id", companyId);
        
        return _productsRepository.Insert(product);
    }

    public Product UpdateProduct(Product product)
    {
        var productExists = _productsRepository.Get(product.Id) is not null;
        if (!productExists) throw new UnknownModelException("Product", "Id", product.Id);
        
        var companyId = product.Supplier?.Id ?? 0;

        var companyExists = _companiesService.CompanyExists(companyId);
        if (!companyExists) throw new UnknownModelException("Company", "Id", companyId);
        
        return _productsRepository.Update(product.Id, product);
    }

    public void DeleteProduct(int productId)
    {
        var product = _productsRepository.Get(productId);
        if (product is null) throw new UnknownModelException("Product", "Id", productId);

        var productResources = _productResourcesRepository.GetProductResources(productId);
        foreach (var productResource in productResources)
        {
            DeleteProductResource(productResource);
        }

        _productsRepository.Delete(productId);
    }


    public (string ContentType, string Name, Stream Content) GetProductImage(int productId, int imageId)
        => GetProductResource(productId, imageId, ProductResourceIsImage);

    public (string ContentType, string Name, Stream Content) GetProductImageById(int imageId)
    {
        var productResource = _productResourcesRepository.Get(imageId);
        if (productResource is null) throw new UnknownModelException("Product Resource", "Id", imageId);

        var content = _contentStorageService.GetContent(productResource.StorageKey);
        return (productResource.ContentType, productResource.Name, content);
    }

    public (string ContentType, string Name, Stream Content)  GetProductDocument(int productId, int documentId)
        => GetProductResource(productId, documentId, ProductResourceIsDocument);
    
    private (string ContentType, string Name, Stream Content) GetProductResource(
        int productId,
        int resourceId,
        Func<ProductResource, bool> resourceTypeCheckFunc)
    {
        var productExists = _productsRepository.Get(productId) is not null;
        if (!productExists) throw new UnknownModelException("Product", "Id", productId);

        var productResource = _productResourcesRepository.Get(resourceId);
        if (productResource is null) throw new UnknownModelException("Product Resource", "Id", resourceId);

        var productResourceTypeIsValid = resourceTypeCheckFunc(productResource);
        if (!productResourceTypeIsValid) throw new UnknownModelException("Product Resource", "Id", resourceId);

        var content = _contentStorageService.GetContent(productResource.StorageKey);
        return (productResource.ContentType, productResource.Name, content);
    }


    public ProductResource AddProductImage(int productId, string contentType, string name, Stream content)
        => AddProductResource(productId, contentType, name, content, ProductResourceIsImage);
    
    public ProductResource AddProductDocument(int productId, string contentType, string name, Stream content)
        => AddProductResource(productId, contentType, name, content, ProductResourceIsDocument);
    
    private ProductResource AddProductResource(
        int productId,
        string contentType,
        string name,
        Stream content,
        Func<ProductResource, bool> resourceTypeCheckFunc)
    {
        var exists = _productsRepository.ProductExists(productId);
        if (!exists) throw new UnknownModelException("Product", "Id", productId);

        var productResource = new ProductResource
        {
            ProductId = productId,
            ContentType = contentType,
            Name = name
        };
        
        var productResourceTypeIsValid = resourceTypeCheckFunc(productResource);
        if (!productResourceTypeIsValid) throw new InvalidModelException("Unsupported Content Type.");

        productResource.StorageKey = _contentStorageService.StoreContent(content);
        return _productResourcesRepository.Insert(productResource);
    }

    public void DeleteProductImage(int productId, int imageId)
        => DeleteProductResource(productId, imageId, ProductResourceIsImage);
    
    public void DeleteProductDocument(int productId, int documentId)
        => DeleteProductResource(productId, documentId, ProductResourceIsDocument);

    private void DeleteProductResource(
        int productId,
        int resourceId,
        Func<ProductResource, bool> resourceTypeCheckFunc)
    {
        var exists = _productsRepository.Get(productId) is not null;
        if (!exists) throw new UnknownModelException("Product", "Id", productId);
        
        var productResource = _productResourcesRepository.Get(resourceId);
        if (productResource is null) throw new UnknownModelException("Product Resource", "Id", resourceId);
        
        var productResourceTypeIsValid = resourceTypeCheckFunc(productResource);
        if (!productResourceTypeIsValid) throw new UnknownModelException("Product Resource", "Id", resourceId);
        
        _contentStorageService.RemoveContent(productResource.StorageKey);
        _productResourcesRepository.Delete(resourceId);
    }

    public string GetAndDeleteProductCloudImage(int productId, int resourceId)
    {
        var exists = _productsRepository.Get(productId) is not null;
        if (!exists) throw new UnknownModelException("Product", "Id", productId);

        var productResource = _productResourcesRepository.Get(resourceId);
        if (productResource is null) throw new UnknownModelException("Product Resource", "Id", resourceId);

        _contentStorageService.RemoveContent(productResource.StorageKey);
        _productResourcesRepository.Delete(resourceId);

        return productResource.Name;
    }

    private void DeleteProductResource(ProductResource productResource)
    {
        _contentStorageService.RemoveContent(productResource.StorageKey);
        _productResourcesRepository.Delete(productResource.Id);
    }

    
    private static bool ProductResourceIsImage(ProductResource productResource)
        => ImageMimeTypes.Contains(productResource.ContentType);

    private static bool ProductResourceIsDocument(ProductResource productResource)
        => DocumentMimeTypes.Contains(productResource.ContentType);
}
