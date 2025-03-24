using IODataLabs.OrderProcessingSystem.Application.DTO;
using IODataLabs.OrderProcessingSystem.Application.Interfaces;
using IODataLabs.OrderProcessingSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace IODataLabs.OrderProcessingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IOpenPayService _openPayService;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(IOpenPayService openPayService, ILogger<PaymentsController> logger)
        {
            _openPayService = openPayService;
            _logger = logger;
        }

        /// <summary>
        ///    Creates a new customer, adds a card, and processes a payment in a single operation
        /// </summary>
        /// <param name="request">CustomerWithCardPaymentRequestDto</param>
        /// <returns>Payment status</returns>
        [HttpPost("ProcessPayment")]
        public async Task<IActionResult> ProcessPayment([FromBody] CustomerWithCardPaymentRequestDto request)
        {
            try
            {
                var payment = await _openPayService.ProcessPaymentAsync(request);
                return Ok(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment");
                return StatusCode(500, new { message = "An error occurred while processing the payment" });
            }
        }
    }
}
