using IODataLabs.OpenPayAdapter;
using IODataLabs.OrderProcessingSystem.Application.DTO;
using IODataLabs.OrderProcessingSystem.Application.Interfaces;
using IODataLabs.OrderProcessingSystem.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Openpay.Entities.Request;

namespace IODataLabs.OrderProcessingSystem.Application.Services
{

    public class OpenPayService : IOpenPayService
    {
        private readonly IOpenPayAdapterService _openPayAdapterService;
        private readonly ILogger<OpenPayService> _logger;
        private readonly string _redirectUrl;
        private readonly string _deviceSessionId;

        public OpenPayService(
            IOpenPayAdapterService openPayAdapterService,
            IConfiguration configuration,
            ILogger<OpenPayService> logger)
        {
            _openPayAdapterService = openPayAdapterService;
            _logger = logger;
            _deviceSessionId = configuration["OpenPay:DeviceSessionId"]
                ?? throw new InvalidOperationException("DeviceSessionId is not configured");
            _redirectUrl = configuration["OpenPay:RedirectUrl"]
                ?? throw new InvalidOperationException("RedirectUrl is not configured");
        }

        public async Task<Payment> ProcessPaymentAsync(CustomerWithCardPaymentRequestDto request)
        {
            try
            {
                _logger.LogInformation("Starting combined customer, card, and payment process");

                // Create customer
                var customer = await _openPayAdapterService.CreateCustomerAsync(new Openpay.Entities.Customer
                {
                    Name = request.Name,
                    Email = request.Email,
                    RequiresAccount = false
                });

                _logger.LogInformation("Customer created with ID: {CustomerId}", customer.Id);

                // Create card
                var card = new Openpay.Entities.Card
                {
                    CardNumber = request.CardNumber,
                    HolderName = request.Name,
                    ExpirationYear = request.ExpirationYear,
                    ExpirationMonth = request.ExpirationMonth,
                    Cvv2 = request.Cvv2,
                    DeviceSessionId = _deviceSessionId
                };

                var createdCard = await _openPayAdapterService.CreateCardTokenAsync(card);

                _logger.LogInformation("Card created with ID: {CustomerId}", createdCard.Id);

                // Create charge request
                var chargeRequest = new ChargeRequest
                {
                    Method = "card",
                    SourceId = createdCard.Id,
                    Amount = new Decimal(100.00),
                    //Currency = request.Currency,
                    Description = $"Order: {request.OrderId}",
                    DeviceSessionId = _deviceSessionId,
                    OrderId = request.OrderId,
                    Use3DSecure = true,
                    RedirectUrl = _redirectUrl,
                    Customer = customer
                };

                var charge = await _openPayAdapterService.CreateChargeAsync(chargeRequest);
                _logger.LogInformation("Charge created with ID: {ChargeId}", charge.Id);

                return new Payment
                {
                    Id = charge.Id,
                    OrderId = request.OrderId,
                    CustomerId = customer.Id,
                    //Amount = new Decimal(100.00),
                    Currency = "MXN",
                    Status = charge.Status ?? "unknown",
                    CreatedAt = charge.CreationDate ?? DateTime.UtcNow,
                    TransactionId = charge.Authorization,
                    ThreeDSecureUrl = charge.PaymentMethod?.Url
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in combined payment process: {Message}", ex.Message);
                throw;
            }
        }
    }
}
