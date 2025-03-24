using IODataLabs.OrderProcessingSystem.Application.DTO;
using IODataLabs.OrderProcessingSystem.Domain.Entities;

namespace IODataLabs.OrderProcessingSystem.Application.Interfaces
{
    public interface IOpenPayService
    {
        Task<PaymentDto> ProcessPaymentAsync(CustomerWithCardPaymentRequestDto paymentRequestDto);
    }
}
