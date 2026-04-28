using AutoMapper;
using FlowerShopOnlineOrderSystem.Models;
using FlowerShopOnlineOrderSystem.ViewModels;

namespace FlowerShopOnlineOrderSystem.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Flower, FlowerViewModel>().ReverseMap();

            CreateMap<Flower, OrderItemInputViewModel>()
                .ForMember(dest => dest.FlowerName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Quantity, opt => opt.Ignore());

            CreateMap<OrderCreateViewModel, Customer>()
                .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.CustomerFirstName.Trim()))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.CustomerLastName.Trim()))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.CustomerEmail.Trim().ToLowerInvariant()))
                .ForMember(dest => dest.Orders, opt => opt.Ignore());

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
                .ForMember(dest => dest.CustomerEmail,
                    opt => opt.MapFrom(src => src.Customer != null ? src.Customer.Email : string.Empty))
                .ForMember(dest => dest.Items,
                    opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<Order, OrderListViewModel>()
                .ForMember(dest => dest.CustomerName,
                    opt => opt.MapFrom(src =>
                        src.Customer != null
                            ? src.Customer.FirstName + " " + src.Customer.LastName
                            : string.Empty))
                .ForMember(dest => dest.CustomerEmail,
                    opt => opt.MapFrom(src => src.Customer != null ? src.Customer.Email : string.Empty));
        }
    }
}
