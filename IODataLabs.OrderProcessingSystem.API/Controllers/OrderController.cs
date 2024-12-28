using IODataLabs.OrderProcessingSystem.Application.Services;
using IODataLabs.OrderProcessingSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace IODataLabs.OrderProcessingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        // Endpoint to create a new order for a customer
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] CreateOrderRequest request)
        {
            try
            {
                var order = await _orderService.CreateOrderAsync(request.CustomerId, request.ProductIds);
                return CreatedAtAction(nameof(GetOrderDetails), new { id = order.OrderId }, order);
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Endpoint to retrieve details for a specific order, including the total price
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderDetails(int id)
        {
            try
            {
                var order = await _orderService.GetOrderDetailsAsync(id);
                return Ok(order);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Endpoint to update an order (e.g., change fulfillment status)
        [HttpPut("{id}")]
        public async Task<ActionResult<Order>> UpdateOrder(int id, [FromBody] bool isFulfilled)
        {
            try
            {
                var updatedOrder = await _orderService.UpdateOrderAsync(id, isFulfilled);
                return Ok(updatedOrder);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Endpoint to delete an order
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }

    public class CreateOrderRequest
    {
        public int CustomerId { get; set; }
        public List<int> ProductIds { get; set; }
    }
}

