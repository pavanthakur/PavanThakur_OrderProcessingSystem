using IODataLabs.OrderProcessingSystem.Application.DTO;

namespace IODataLabs.OrderProcessingSystem.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<int> CreateCustomerAsync(CreateCustomerRequestDto customerDto);
        Task<CustomerDto> GetCustomerByIdAsync(int customerId);
        Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
        Task<CustomerDto> GetCustomerWithOrdersAsync(int customerId);
        void VerifyExceptionLoggedInService();
    }
}
