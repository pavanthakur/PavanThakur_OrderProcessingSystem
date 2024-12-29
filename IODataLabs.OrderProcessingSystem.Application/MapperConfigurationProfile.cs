using AutoMapper;
using IODataLabs.OrderProcessingSystem.Application.DTO;
using IODataLabs.OrderProcessingSystem.Domain.Entities;

namespace IODataLabs.OrderProcessingSystem.Application
{
    public class MapperConfigurationProfile : Profile
    {
        public MapperConfigurationProfile()
        {
            CreateMap<Customer, CustomerDto>().ReverseMap(); ;
            CreateMap<Product, ProductDto>().ReverseMap(); ;
            CreateMap<Order, OrderDto>().ReverseMap(); ;
            CreateMap<OrderProduct, OrderProductDto>().ReverseMap(); ;
        }
    }
}
