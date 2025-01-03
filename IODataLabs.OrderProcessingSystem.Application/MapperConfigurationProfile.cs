using AutoMapper;
using IODataLabs.OrderProcessingSystem.Application.DTO;
using IODataLabs.OrderProcessingSystem.Domain.Entities;

namespace IODataLabs.OrderProcessingSystem.Application
{
    public class MapperConfigurationProfile : Profile
    {
        public MapperConfigurationProfile()
        {
            CreateMap<CreateCustomerRequestDto, Customer>();
            CreateMap<Customer, CustomerDto>().ForMember(dest => dest.OrderDtos, opt => opt.MapFrom(src => src.Orders));
            CreateMap<Product, ProductDto>();
            CreateMap<Order, OrderDto>();
            CreateMap<OrderProduct, OrderProductDto>();
        }
    }
}
