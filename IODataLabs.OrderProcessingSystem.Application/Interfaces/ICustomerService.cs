using IODataLabs.OrderProcessingSystem.Application.DTO;
using IODataLabs.OrderProcessingSystem.Application.Request;

namespace IODataLabs.OrderProcessingSystem.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<int> CreateCustomerAsync(CreateCustomerRequest customerDto);
        Task<CustomerDto> GetCustomerByIdAsync(int customerId);
        Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
        Task<CustomerDto> GetCustomerWithOrdersAsync(int customerId);
        void VerifyExceptionLoggedInService();
    }
}
