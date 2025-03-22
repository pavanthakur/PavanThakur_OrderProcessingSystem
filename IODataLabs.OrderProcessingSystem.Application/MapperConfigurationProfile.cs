using AutoMapper;
using IODataLabs.OrderProcessingSystem.Application.DTO;
using IODataLabs.OrderProcessingSystem.Domain.Entities;

namespace IODataLabs.OrderProcessingSystem.Application
{
    public class MapperConfigurationProfile : Profile
    {
        public MapperConfigurationProfile()
        {
            CreateMap<CustomerWithCardPaymentRequestDto, Customer>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<CreateCustomerRequestDto, Customer>();
            CreateMap<UpdateCustomerRequestDto, Customer>();
            CreateMap<Customer, CustomerDto>().ForMember(dest => dest.OrderDtos, opt => opt.MapFrom(src => src.Orders));
            CreateMap<Product, ProductDto>();
            CreateMap<Order, OrderDto>();
            CreateMap<OrderProduct, OrderProductDto>();
        }
    }
}
