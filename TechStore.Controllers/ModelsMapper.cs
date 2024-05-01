using AutoMapper;

using TechStore.Controllers.Models.Companies;
using TechStore.Controllers.Models.Orders;
using TechStore.Controllers.Models.Products;
using TechStore.Controllers.Models.Users;

using TechStore.Domain.Models.Orders;
using TechStore.Domain.Models.Products;
using TechStore.Domain.Models.Users;

namespace TechStore.Controllers;

public class ModelsMapper : Profile
{
    public ModelsMapper()
    {
        CreateClientsMaps();
        CreateAdminsMaps();
        CreateCompaniesMaps();
        CreateProductsMaps();
        CreateOrderItemsMaps();
        CreateOrderReviewsMaps();
    }

    private void CreateAdminsMaps()
    {
        CreateMap<Admin, ApiAdmin>()
            .ForMember(
                d => d.Id,
                _ => _.MapFrom(s => s.Id))
            .ForMember(
                d => d.FirstName,
                _ => _.MapFrom(s => s.FirstName))
            .ForMember(
                d => d.LastName,
                _ => _.MapFrom(s => s.LastName))
            .ForMember(
                d => d.Email,
                _ => _.MapFrom(s => s.Email))
            .ForMember(
                d => d.UserName,
                _ => _.MapFrom(s => s.UserName))
            .ForMember(
                d => d.Password,
                _ => _.MapFrom(s => s.Password));

        CreateMap<ApiAdmin, Admin>()
            .ForMember(
                d => d.Id,
                _ => _.MapFrom(s => s.Id))
            .ForMember(
                d => d.FirstName,
                _ => _.MapFrom(s => s.FirstName))
            .ForMember(
                d => d.LastName,
                _ => _.MapFrom(s => s.LastName))
            .ForMember(
                d => d.Email,
                _ => _.MapFrom(s => s.Email))
            .ForMember(
                d => d.UserName,
                _ => _.MapFrom(s => s.UserName))
            .ForMember(
                d => d.Password,
                _ => _.MapFrom(s => s.Password));
    }

    private void CreateClientsMaps()
    {
        CreateMap<Client, ApiClient>()
            .ForMember(
                d => d.Id,
                _ => _.MapFrom(s => s.Id))
            .ForMember(
                d => d.FirstName,
                _ => _.MapFrom(s => s.FirstName))
            .ForMember(
                d => d.LastName,
                _ => _.MapFrom(s => s.LastName))
            .ForMember(
                d => d.Email,
                _ => _.MapFrom(s => s.Email))
            .ForMember(
                d => d.Addresses,
                _ => _.MapFrom(s => s.Addresses == null ? null : s.Addresses.Select(x => x.Address).ToList()));

        CreateMap<AuthenticationResult, ApiAuthenticationResult>()
            .ForMember(
                d => d.Username,
                _ => _.MapFrom(s => s.Username))
            .ForMember(
                d => d.Token,
                _ => _.MapFrom(s => s.Token))
            .ForMember(
                d => d.ExpiresUtc,
                _ => _.MapFrom(s => s.ExpiresUtc));
    }

    private void CreateCompaniesMaps()
    {
        CreateMap<Company, ApiCompany>()
            .ForMember(
                d => d.Id,
                _ => _.MapFrom(s => s.Id))
            .ForMember(
                d => d.Name,
                _ => _.MapFrom(s => s.Name));
    }
    
    private void CreateProductsMaps()
    {
        CreateMap<Product, ApiProduct>()
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
                _ => _.MapFrom(s => s.UnitPrice))
            .ForMember(
                d => d.UnitsAvailable,
                _ => _.MapFrom(s => s.UnitsAvailable))
            .ForMember(
                d => d.ProducingCountry,
                _ => _.MapFrom(s => s.ProducingCountry))
            .ForMember(
                d => d.Images,
                _ => _.MapFrom(s => s.Images))
            .ForMember(
                d => d.Documents,
                _ => _.MapFrom(s => s.Documents))
            .ForMember(
                d => d.Supplier,
                _ => _.MapFrom(s => s.Supplier));
        
        CreateMap<ProductResource, ApiProductResource>()
            .ForMember(
                d => d.Id,
                _ => _.MapFrom(s => s.Id))
            .ForMember(
                d => d.Name,
                _ => _.MapFrom(s => s.Name));
    }

    private void CreateOrderItemsMaps()
    {
        CreateMap<Order, ApiOrder>()
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
                _ => _.MapFrom(s => s.Items))
            .ForMember(
                d => d.Review,
                _ => _.MapFrom(s => s.Review));
            
        CreateMap<OrderItem, ApiOrderItem>()
            .ForMember(
                d => d.Id,
                _ => _.MapFrom(s => s.Id))
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

        CreateMap<OrderReview, ApiOrderReview>()
            .ForMember(
                d => d.Rate,
                _ => _.MapFrom(s => s.Rate))
            .ForMember(
                d => d.Comment,
                _ => _.MapFrom(s => s.Comment));
    }

    private void CreateOrderReviewsMaps()
    {
        CreateMap<OrderReview, ApiOrderReview>()
            .ForMember(
                d => d.Rate,
                _ => _.MapFrom(s => s.Rate))
            .ForMember(
                d => d.Comment,
                _ => _.MapFrom(s => s.Comment));
        
        CreateMap<ApiOrderReview, OrderReview>()
            .ForMember(
                d => d.Rate,
                _ => _.MapFrom(s => s.Rate))
            .ForMember(
                d => d.Comment,
                _ => _.MapFrom(s => s.Comment));
    }
}