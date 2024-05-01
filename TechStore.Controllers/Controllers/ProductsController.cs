using AutoMapper;

using Azure.Storage.Blobs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TechStore.Business.Exceptions;
using TechStore.Controllers.Models.Products;
using TechStore.Domain.Models.Products;
using TechStore.Domain.Services;
using TechStore.Domain.Types;

namespace TechStore.Controllers.Controllers;

[ApiController]
[Route("products")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IProductsService _productsService;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName = "work-images";
    private readonly string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=diplomaimages;AccountKey=uA5KzJtmS6TYvT+oc8boTL2bqRoJl6DxUerY2jR8V1kRxacLoHsiFyi2j5CQS1DIt/IY1VikQ+ue+ASttJXmQA==;EndpointSuffix=core.windows.net";


    public ProductsController(
        IMapper mapper,
        IProductsService productsService)
    {
        ArgumentNullException.ThrowIfNull(mapper);
        _mapper = mapper;
        
        ArgumentNullException.ThrowIfNull(productsService);
        _productsService = productsService;

        string connectionString = ConnectionString;

        _blobServiceClient = new BlobServiceClient(connectionString);
    }

    [HttpGet("{productId}")]
    public ActionResult<ApiProduct> GetProductById(int productId)
    {
        try
        {
            var product = _productsService.GetProductById(productId);
            return Ok(_mapper.Map<ApiProduct>(product));
        }
        catch (UnknownModelException exception)
        {
            return NotFound(exception.Message);
        }
    }

    [HttpGet("all")]
    [AllowAnonymous]
    public ActionResult<ApiProduct> GetAllProducts()
    {
        try
        {
            var product = _productsService.GetAllProducts();
            return Ok(_mapper.Map<IEnumerable<ApiProduct>>(product));
        }
        catch (UnknownModelException exception)
        {
            return NotFound(exception.Message);
        }
    }

    [HttpPost]
    [Authorize(Roles = UserRole.Admin)]
    public ActionResult<ApiProduct> CreateProduct(ApiProductCreateRequest request)
    {
        try
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Model = request.Model,
                UnitPrice = request.UnitPrice,
                UnitsAvailable = request.UnitsAvailable,
                ProducingCountry = request.ProducingCountry,
                Supplier = new Company { Id = request.SupplierId },
            };
                
            product = _productsService.CreateProduct(product);
            return Ok(_mapper.Map<ApiProduct>(product));
        }
        catch (UnknownModelException exception)
        {
            return NotFound(exception.Message);
        }
        catch (InvalidModelException exception)
        {
            return BadRequest(exception.Errors);
        }
    }

    [HttpPut("{productId}")]
    [Authorize(Roles = UserRole.Admin)]
    public ActionResult<ApiProduct> UpdateProduct(int productId, ApiProductUpdateRequest request)
    {
        try
        {
            var product = new Product
            {
                Id = productId,
                Name = request.Name,
                Description = request.Description,
                Model = request.Model,
                UnitPrice = request.UnitPrice,
                UnitsAvailable = request.UnitsAvailable,
                ProducingCountry = request.ProducingCountry,
                Supplier = new Company { Id = request.SupplierId },
            };
            
            product = _productsService.UpdateProduct(product);
            return Ok(_mapper.Map<ApiProduct>(product));
        }
        catch (UnknownModelException exception)
        {
            return NotFound(exception.Message);
        }
        catch (InvalidModelException exception)
        {
            return BadRequest(exception.Errors);
        }
    }

    [HttpDelete("{productId}")]
    [Authorize(Roles = UserRole.Admin)]
    public IActionResult DeleteProduct(int productId)
    {
        try
        {
            _productsService.DeleteProduct(productId);
            return NoContent();
        }
        catch (UnknownModelException exception)
        {
            return NotFound(exception.Message);
        }
    }

    
    [HttpGet("{productId}/images/{imageId}")]
    [AllowAnonymous]
    public IActionResult GetProductImage(int productId, int imageId)
    {
        try
        {
            var (contentType, _, content) = _productsService.GetProductImage(productId, imageId);
            return new FileStreamResult(content, contentType);
        }
        catch(UnknownModelException exception)
        {
            return NotFound(exception.Message);
        }
    }

    [HttpPost("{productId}/images")]
    [DisableRequestSizeLimit]
    [Authorize(Roles = UserRole.Admin)]
    public ActionResult<ApiProductResource> AddProductImage(int productId, IFormFile file)
    {
        try
        {
            using var content = file.OpenReadStream();
            var productImage = _productsService.AddProductImage(productId, file.ContentType, file.FileName, content);
            content.Close();

            return _mapper.Map<ApiProductResource>(productImage);
        }
        catch(UnknownModelException exception)
        {
            return NotFound(exception.Message);
        }
        catch(InvalidModelException exception)
        {
            return BadRequest(exception.Errors);
        }
    }

    [HttpDelete("{productId}/images/{imageId}")]
    [Authorize(Roles = UserRole.Admin)]
    public IActionResult DeleteProductImage(int productId, int imageId)
    {
        try
        {
            _productsService.DeleteProductImage(productId, imageId);
            return NoContent();
        }
        catch(UnknownModelException exception)
        {
            return NotFound(exception.Message);
        }
    }
    
    
    [HttpGet("{productId}/documents/{documentId}")]
    [AllowAnonymous]
    public IActionResult GetProductDocument(int productId, int documentId)
    {
        try
        {
            var (contentType, name, content) = _productsService.GetProductDocument(productId, documentId);
            return new FileStreamResult(content, contentType) { FileDownloadName = name };
        }
        catch(UnknownModelException exception)
        {
            return NotFound(exception.Message);
        }
    }

    [HttpPost("{productId}/documents")]
    [DisableRequestSizeLimit]
    [Authorize(Roles = UserRole.Admin)]
    public ActionResult<ApiProductResource> AddProductDocument(int productId, IFormFile file)
    {
        try
        {

            using var content = file.OpenReadStream();
            var productDocument = _productsService.AddProductDocument(productId, file.ContentType, file.FileName, content);
            content.Close();

            return _mapper.Map<ApiProductResource>(productDocument);
        }
        catch(UnknownModelException exception)
        {
            return NotFound(exception.Message);
        }
        catch(InvalidModelException exception)
        {
            return BadRequest(exception.Errors);
        }
    }

    [HttpDelete("{productId}/documents/{documentId}")]
    [Authorize(Roles = UserRole.Admin)]
    public IActionResult DeleteProductDocument(int productId, int documentId)
    {
        try
        {
            _productsService.DeleteProductDocument(productId, documentId);
            return NoContent();
        }
        catch(UnknownModelException exception)
        {
            return NotFound(exception.Message);
        }
    }

    [HttpPost("{productId}/cloudImages/upload")]
    [Authorize(Roles = UserRole.Admin)]
    public async Task<IActionResult> UploadImage(int productId, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No image file found.");
        }

        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

        BlobClient blobClient = containerClient.GetBlobClient(fileName);
        var response = await blobClient.UploadAsync(file.OpenReadStream(), true);

        using (MemoryStream memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            var productDocument = _productsService.AddProductImage(productId, file.ContentType, blobClient.Name, memoryStream);
            memoryStream.Close();
        }

        return Ok(blobClient);
    }


    [HttpGet("{productId}/cloudImages/{imageId}")]
    [Authorize(Roles = UserRole.Admin)]
    public async Task<IActionResult> GetImage(int productId, int imageId)
    {
        string imageName;
        try
        {
            var (contentType, name, content) = _productsService.GetProductImage(productId, imageId);
            imageName = name;
        }
        catch (UnknownModelException exception)
        {
            return NotFound(exception.Message);
        }

        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

        BlobClient blobClient = containerClient.GetBlobClient(imageName);

        if (!await blobClient.ExistsAsync())
        {
            return NotFound("Image not found.");
        }

        var response = await blobClient.DownloadAsync();
        var imageBytes = new byte[response.Value.ContentLength];
        await response.Value.Content.ReadAsync(imageBytes, 0, imageBytes.Length);

        return File(imageBytes, response.Value.ContentType);
    }

    [HttpDelete("{productId}/deleteCloudImage/{imageId}")]
    [Authorize(Roles = UserRole.Admin)]
    public async Task<IActionResult> DeleteImage(int productId, int imageId)
    {
        string imageName;
        try
        {
            imageName = _productsService.GetAndDeleteProductCloudImage(productId, imageId);
        }
        catch (UnknownModelException exception)
        {
            return NotFound(exception.Message);
        }

        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

        BlobClient blobClient = containerClient.GetBlobClient(imageName);

        if (!await blobClient.ExistsAsync())
        {
            return NotFound("Image not found.");
        }

        await blobClient.DeleteAsync();

        return NoContent();
    }
}
