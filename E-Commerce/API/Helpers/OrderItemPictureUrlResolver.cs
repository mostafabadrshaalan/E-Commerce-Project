using API.DTOs;
using AutoMapper;
using Core.Entities.OrderAggergate;
using Microsoft.IdentityModel.Tokens;

namespace API.Helpers
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration configuration;

        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            return source.ProductItem.PictureUrl.IsNullOrEmpty()
                   ? ""
                   : configuration["ApiUrl"] + source.ProductItem.PictureUrl;
        }
    }
}
