using AutoMapper;
using Domain.OrderModels;
using Shared.OrdersDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile() 
        {
            CreateMap<Address,AddressDto>().ReverseMap();

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.product.ProductId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.product.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.product.PictureUrl));

            CreateMap<Order, OrderResultDto>()
                .ForMember(d => d.PaymentStatus, o => o.MapFrom(s => s.PaymentStatus.ToString()))
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.Total, o => o.MapFrom(s => s.SubTotal + s.DeliveryMethod.Cost));


            CreateMap<DeliveryMethod, DeliveryMethodDto>();


        }
    }
}
