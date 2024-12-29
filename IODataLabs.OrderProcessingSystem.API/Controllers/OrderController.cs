using IODataLabs.OrderProcessingSystem.Application.Interfaces;
using IODataLabs.OrderProcessingSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace IODataLabs.OrderProcessingSystem.API.Controllers
{
    /// <summary>
    /// Controller to manage order-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderController"/> class.
        /// </summary>
        /// <param name="orderService">The order service.</param>
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Endpoint to Create order for a customer
        /// </summary>
        /// <remarks>Create order.</remarks>  
        /// <returns></returns>
        /// <response code="201">Create order</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] CreateOrderRequest request)
        {
            try
            {
                var order = await _orderService.CreateOrderAsync(request.CustomerId, request.ProductIds);
                return CreatedAtAction(nameof(CreateOrder), new { id = order.OrderId }, order);
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

        /// <summary>
        /// Endpoint to retrieve details for a specific order, including the total price
        /// </summary>
        /// <remarks>This is the first step to 
        /// retrieve a specific Order</remarks>  
        /// <param name="id">Order id</param>
        /// <returns>Order</returns>
        [HttpGet("{id}", Name = nameof(GetOrderDetailsById))]
        public async Task<ActionResult<Order>> GetOrderDetailsById(int id)
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

        /// <summary>
        /// Endpoint to update an order (e.g., change fulfillment status)
        /// </summary>
        /// <remarks>This is to update an existing order.</remarks>
        /// <param name="id">Order id</param>
        /// <param name="isFulfilled">isFulfilled</param>
        /// <returns>Order</returns>
        [HttpPut("{id}/UpdateOrder", Name = nameof(UpdateOrder))]
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

        /// <summary>
        /// Endpoint to delete an existing order
        /// </summary>
        /// <remarks>This is to delete an existing order.</remarks>
        /// <param name="id">Order id</param>
        /// <returns>Deleted status</returns>
        /// <response code="204">Return 204 No Content if deletion is successful</response>
        [HttpDelete("{id}/DeleteOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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

    /// <summary>
    /// Request model for creating an order.
    /// </summary>
    public class CreateOrderRequest
    {
        /// <summary>
        /// Gets or sets the customer ID.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the list of product IDs.
        /// </summary>
        public required List<int> ProductIds { get; set; }
    }
}

