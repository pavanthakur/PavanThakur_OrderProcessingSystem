using IODataLabs.OrderProcessingSystem.Application.DTO;

namespace IODataLabs.OrderProcessingSystem.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerDto> CreateCustomerAsync(CustomerDto customerDto);
        Task<CustomerDto> GetCustomerByIdAsync(int customerId);
        Task DeleteCustomerAsync(int customerId);
        Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
        Task<CustomerDto> GetCustomerWithOrdersAsync(int customerId);
        Task<CustomerDto> UpdateCustomerAsync(int customerId, CustomerDto updatedCustomer);
        void VerifyExceptionLoggedInService();
    }
}
