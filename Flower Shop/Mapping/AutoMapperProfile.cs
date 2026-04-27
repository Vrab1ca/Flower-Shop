using AutoMapper;
using FlowerShopOnlineOrderSystem.Models;
using FlowerShopOnlineOrderSystem.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FlowerShopOnlineOrderSystem.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Flower, FlowerViewModel>().ReverseMap();

            CreateMap<OrderItem, OrderItemViewModel>()
                .ForMember(dest => dest.FlowerName,
                    opt => opt.MapFrom(src => src.Flower != null ? src.Flower.Name : string.Empty))
                .ForMember(dest => dest.Price,
                    opt => opt.MapFrom(src => src.Flower != null ? src.Flower.Price : 0));

            CreateMap<Order, OrderDetailsViewModel>()
                .ForMember(dest => dest.CustomerName,
                    opt => opt.MapFrom(src =>
                        src.Customer != null
                            ? src.Customer.FirstName + " " + src.Customer.LastName
                            : string.Empty))
                .ForMember(dest => dest.Items,
                    opt => opt.MapFrom(src => src.OrderItems));
        }
    }
}