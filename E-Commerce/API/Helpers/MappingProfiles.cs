using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.OrderAggergate;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(productDto => productDto.Brand, option => option.MapFrom(product => product.Brand.Name))
                .ForMember(productDto => productDto.Category, option => option.MapFrom(product => product.Category.Name))
                .ForMember(productDto => productDto.PictureUrl, option => option.MapFrom<ProductUrlResolver>())
                .ReverseMap();

            CreateMap<BasketItem, BasketItemDto>().ReverseMap();
            CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();

            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<ShippingAddress, ShippingAddressDto>().ReverseMap();

            CreateMap<Order, OrderDetailsDto>()
                .ForMember(dest => dest.DeliveryMethod, option => option.MapFrom(src => src.DeliveryMethod.ShortName))
                .ForMember(dest => dest.ShippingPrice, option => option.MapFrom(src => src.DeliveryMethod.Price))
                .ForMember(dest => dest.OrderStatus, option => option.MapFrom(src => src.OrderStatus.ToString()))
                .ReverseMap();

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductId, option => option.MapFrom(src => src.ProductItem.ProductId))
                .ForMember(dest => dest.ProductName, option => option.MapFrom(src => src.ProductItem.ProductName))
                .ForMember(dest => dest.PictureUrl, option => option.MapFrom(src => src.ProductItem.PictureUrl))
                .ForMember(dest => dest.PictureUrl, option => option.MapFrom<OrderItemPictureUrlResolver>())
                .ReverseMap();



        }
    }
}
