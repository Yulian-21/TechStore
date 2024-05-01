using System.Linq;

using AutoMapper;

using TechStore.DB.SQLite.Entities.Orders;
using TechStore.DB.SQLite.Entities.Products;
using TechStore.DB.SQLite.Entities.Users;

using TechStore.Domain.Models.Orders;
using TechStore.Domain.Models.Products;
using TechStore.Domain.Models.Users;

namespace TechStore.DB.SQLite;

public class ModelsMapper : Profile
{
    public ModelsMapper()
    {
        CreateAdminsMaps();
        CreateClientsMaps();
        CreateCompaniesMaps();
        CreateProductsMaps();
        CreateSuppliersMaps();
        CreateOrdersMaps();
    }

    private void CreateAdminsMaps()
    {
        CreateMap<DbAdmin, Admin>()
            .ForMember(
                d => d.UserName,
                _ => _.MapFrom(s => s.UserName))
            .ForMember(
                d => d.Email,
                _ => _.MapFrom(s => s.Email))
            .ForMember(
                d => d.Password,
                _ => _.MapFrom(s => s.Password))
            .ForMember(
                d => d.FirstName,
                _ => _.MapFrom(s => s.FirstName))
            .ForMember(
                d => d.LastName,
                _ => _.MapFrom(s => s.LastName));

        CreateMap<Admin, DbAdmin>()
            .ForMember(
                d => d.UserName,
                _ => _.MapFrom(s => s.UserName))
            .ForMember(
                d => d.Email,
                _ => _.MapFrom(s => s.Email))
            .ForMember(
                d => d.Password,
                _ => _.MapFrom(s => s.Password))
            .ForMember(
                d => d.FirstName,
                _ => _.MapFrom(s => s.FirstName))
            .ForMember(
                d => d.LastName,
                _ => _.MapFrom(s => s.LastName));
    }
    
    private void CreateClientsMaps()
    {
        CreateMap<DbClient, Client>()
            .ForMember(
                d => d.Id,
                _ => _.MapFrom(s => s.Id))
            .ForMember(
                d => d.Email,
                _ => _.MapFrom(s => s.Email))
            .ForMember(
                d => d.Password,
                _ => _.MapFrom(s => s.Password))
            .ForMember(
                d => d.FirstName,
                _ => _.MapFrom(s => s.FirstName))
            .ForMember(
                d => d.LastName,
                _ => _.MapFrom(s => s.LastName))
            .ForMember(
                d => d.Addresses,
                _ => _.MapFrom(s => s.ShippingAddresses));
        
        CreateMap<Client, DbClient>()
            .ForMember(
                d => d.Id,
                _ => _.MapFrom(s => s.Id))
            .ForMember(
                d => d.Email,
                _ => _.MapFrom(s => s.Email))
            .ForMember(
                d => d.Password,
                _ => _.MapFrom(s => s.Password))
            .ForMember(
                d => d.FirstName,
                _ => _.MapFrom(s => s.FirstName))
            .ForMember(
                d => d.LastName,
                _ => _.MapFrom(s => s.LastName))
            .ForMember(
                d => d.ShippingAddresses,
                _ => _.MapFrom(s => s.Addresses));

        CreateMap<DbClientShippingAddress, ShippingAddress>()
            .ForMember(
                d => d.Id,
                _ => _.MapFrom(s => s.Id))
            .ForMember(
                d => d.Address,
                _ => _.MapFrom(s => s.Address));
        
        CreateMap<ShippingAddress, DbClientShippingAddress>()
            .ForMember(
                d => d.Id,
                _ => _.MapFrom(s => s.Id))
            .ForMember(
                d => d.Address,
                _ => _.MapFrom(s => s.Address));
    }

    private void CreateCompaniesMaps()
    {
        CreateMap<DbCompany, Company>()
            .ForMember(
                d => d.Id,
                _ => _.MapFrom(s => s.Id))
            .ForMember(
                d => d.Name,
                _ => _.MapFrom(s => s.Name));
        
        CreateMap<Company, DbCompany>()
            .ForMember(
                d => d.Id,
                _ => _.MapFrom(s => s.Id))
            .ForMember(
                d => d.Name,
                _ => _.MapFrom(s => s.Name));
    }
    
    private void CreateProductsMaps()
    {
        CreateMap<DbProduct, Product>()
            .ForMember(
                d => d.Id,
                _ => _.MapFrom(s => s.Id))
            .ForMember(
                d => d.Name,
                _ => _.MapFrom(s => s.Name))
            .ForMember(
                d => d.Description,
                _ => _.MapFrom(s => s.Description))
            .ForMember(
                d => d.Model,
                _ => _.MapFrom(s => s.Model))
            .ForMember(
                d => d.UnitPrice,
                _ => _.MapFrom(s => s.Price))
            .ForMember(
                d => d.UnitsAvailable,
                _ => _.MapFrom(s => s.Available))
            .ForMember(
                d => d.ProducingCountry,
                _ => _.MapFrom(s => s.Country))
            .ForMember(
                d => d.Supplier,
                _ => _.MapFrom(s => s.Supplier));
        
        CreateMap<Product, DbProduct>()
            .ForMember(
                d => d.Id,
                _ => _.MapFrom(s => s.Id))
            .ForMember(
                d => d.Name,
                _ => _.MapFrom(s => s.Name))
            .ForMember(
                d => d.Description,
                _ => _.MapFrom(s => s.Description))
            .ForMember(
                d => d.Model,
                _ => _.MapFrom(s => s.Model))
            .ForMember(
                d => d.Price,
                _ => _.MapFrom(s => s.UnitPrice))
            .ForMember(
                d => d.Available,
                _ => _.MapFrom(s => s.UnitsAvailable))
            .ForMember(
                d => d.Country,
                _ => _.MapFrom(s => s.ProducingCountry))
            .ForMember(
                d => d.Supplier,
                _ => _.MapFrom(s => s.Supplier));


        CreateMap<DbProductResource, ProductResource>()
            .ForMember(
                d => d.Id,
                _ => _.MapFrom(s => s.Id))
            .ForMember(
                d => d.ProductId,
                _ => _.MapFrom(s => s.ProductId))
            .ForMember(
                d => d.ContentType,
                _ => _.MapFrom(s => s.ContentType))
            .ForMember(
                d => d.StorageKey,
                _ => _.MapFrom(s => s.StorageKey));
        
        CreateMap<ProductResource, DbProductResource>()
            .ForMember(
                d => d.Id,
                _ => _.MapFrom(s => s.Id))
            .ForMember(
                d => d.ProductId,
                _ => _.MapFrom(s => s.ProductId))
            .ForMember(
                d => d.ContentType,
                _ => _.MapFrom(s => s.ContentType))
            .ForMember(
                d => d.StorageKey,
                _ => _.MapFrom(s => s.StorageKey));
    }
    
    private void CreateSuppliersMaps()
    {
        CreateMap<DbCompany, Company>()
            .ForMember(
                d => d.Id,
                _ => _.MapFrom(s => s.Id))
            .ForMember(
                d => d.Name,
                _ => _.MapFrom(s => s.Name));
    }

    private void CreateOrdersMaps()
    {
        CreateMap<Order, DbOrder>()
            .ForMember(
                d => d.Id,
                _ => _.MapFrom(s => s.Id))
            .ForMember(
                d => d.ClientId,
                _ => _.MapFrom(s => s.ClientId))
            .ForMember(
                d => d.OrderedAt,
                _ => _.MapFrom(s => s.OrderedAt))
            .ForMember(
                d => d.CompletedAt,
                _ => _.MapFrom(s => s.CompletedAt));
        
        CreateMap<DbOrder, Order>()
            .ForMember(
                d => d.Id,
                _ => _.MapFrom(s => s.Id))
            .ForMember(
                d => d.ClientId,
                _ => _.MapFrom(s => s.ClientId))
            .ForMember(
                d => d.OrderedAt,
                _ => _.MapFrom(s => s.OrderedAt))
            .ForMember(
                d => d.CompletedAt,
                _ => _.MapFrom(s => s.CompletedAt))
            .ForMember(
                d => d.Items,
                _ => _.MapFrom(s => s.OrderItems))
            .ForMember(
                d => d.Review,
                _ => _.MapFrom(s => s.Reviews.Any() ? s.Reviews[0] : null));
            
        CreateMap<DbOrderItem, OrderItem>()
            .ForMember(
                d => d.Id,
                _ => _.MapFrom(s => s.Id))
            .ForMember(
                d => d.OrderId,
                _ => _.MapFrom(s => s.OrderId))
            .ForMember(
                d => d.ProductId,
                _ => _.MapFrom(s => s.ProductId))
            .ForMember(
                d => d.Qty,
                _ => _.MapFrom(s => s.Qty))
            .ForMember(
                d => d.Price,
                _ => _.MapFrom(s => s.Price))
            .ForMember(
                d => d.Comment,
                _ => _.MapFrom(s => s.Comment));
        
        CreateMap<OrderItem, DbOrderItem>()
            .ForMember(
                d => d.Id,
                _ => _.MapFrom(s => s.Id))
            .ForMember(
                d => d.OrderId,
                _ => _.MapFrom(s => s.OrderId))
            .ForMember(
                d => d.ProductId,
                _ => _.MapFrom(s => s.ProductId))
            .ForMember(
                d => d.Qty,
                _ => _.MapFrom(s => s.Qty))
            .ForMember(
                d => d.Price,
                _ => _.MapFrom(s => s.Price))
            .ForMember(
                d => d.Comment,
                _ => _.MapFrom(s => s.Comment));
        
        
        CreateMap<DbOrderReview, OrderReview>()
            .ForMember(
                d => d.Id,
                _ => _.MapFrom(s => s.Id))
            .ForMember(
                d => d.OrderId,
                _ => _.MapFrom(s => s.OrderId))
            .ForMember(
                d => d.Rate,
                _ => _.MapFrom(s => s.Rate))
            .ForMember(
                d => d.Comment,
                _ => _.MapFrom(s => s.Comment));
        
        CreateMap<OrderReview, DbOrderReview>()
            .ForMember(
                d => d.Id,
                _ => _.MapFrom(s => s.Id))
            .ForMember(
                d => d.OrderId,
                _ => _.MapFrom(s => s.OrderId))
            .ForMember(
                d => d.Rate,
                _ => _.MapFrom(s => s.Rate))
            .ForMember(
                d => d.Comment,
                _ => _.MapFrom(s => s.Comment));

    }
}