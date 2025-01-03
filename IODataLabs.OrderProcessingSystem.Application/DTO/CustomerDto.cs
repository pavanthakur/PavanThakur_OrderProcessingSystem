using IODataLabs.OrderProcessingSystem.Domain.Entities;

namespace IODataLabs.OrderProcessingSystem.Application.DTO
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public List<OrderDto> OrderDtos { get; set; } = new List<OrderDto>();
    }
}
