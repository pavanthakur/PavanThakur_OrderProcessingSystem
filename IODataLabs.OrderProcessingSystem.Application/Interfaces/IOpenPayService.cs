using IODataLabs.OrderProcessingSystem.Application.DTO;
using IODataLabs.OrderProcessingSystem.Domain.Entities;

namespace IODataLabs.OrderProcessingSystem.Application.Interfaces
{
    public interface IOpenPayService
    {
        Task<Payment> ProcessPaymentAsync(CustomerWithCardPaymentRequestDto paymentRequestDto);
    }
}
