using IODataLabs.OrderProcessingSystem.Application.DTO;
using IODataLabs.OrderProcessingSystem.Application.Interfaces;
using IODataLabs.OrderProcessingSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace IODataLabs.OrderProcessingSystem.API.Controllers
{
    /// <summary>
    /// Controller to manage customer-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerController"/> class.
        /// </summary>
        /// <param name="customerService">The customer service.</param>
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Endpoint to retrieve all customers
        /// </summary>
        /// <remarks>Retrieve all customers.</remarks>  
        /// <returns></returns>
        /// <response code="200">Returns all customers</response>
        [HttpGet("GetAllCustomers", Name = nameof(GetAllCustomers))]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAllCustomers()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }

        /// <summary>
        /// Endpoint to verify if exception is getting logged from service class
        /// </summary>
        /// <remarks>This is to test if logs are working in service method.</remarks>  
        /// <returns>BadRequest</returns>
        /// <response code="500">Internal server error</response>
        [HttpGet("VerifyExceptionLoggedInService", Name = nameof(VerifyExceptionLoggedInService))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult VerifyExceptionLoggedInService()
        {
            _customerService.VerifyExceptionLoggedInService();
            return BadRequest("Customer service thrown some exception please check logs.");
        }

        /// <summary>
        /// Endpoint to retrieve a specific customer and their orders
        /// </summary>
        /// <remarks>This is the first step to 
        /// retrieve a specific customer</remarks>  
        /// <param name="id">Customer id</param>
        /// <returns>Customer</returns>
        [HttpGet("{id}", Name = nameof(GetCustomerById))]
        public async Task<ActionResult<CustomerDto>> GetCustomerById(int id)
        {
            try
            {
                var customer = await _customerService.GetCustomerWithOrdersAsync(id);
                return Ok(customer);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Endpoint to create a new customer
        /// </summary>
        /// <remarks>This is create a new customer</remarks>  
        /// <param name="customer">customer</param>
        /// <returns>Customer</returns>
        [HttpPost("{Customer}/CreateCustomer")]
        public async Task<ActionResult<CustomerDto>> CreateCustomer([FromBody] CustomerDto customer)
        {
            if (customer == null)
            {
                return BadRequest("Customer data is required.");
            }

            var createdCustomer = await _customerService.CreateCustomerAsync(customer);
            return CreatedAtAction(nameof(CreateCustomer), new { id = createdCustomer.CustomerId }, createdCustomer);
        }

        /// <summary>
        /// Endpoint to update an existing customer
        /// </summary>
        /// <remarks>This is to update an existing customer.</remarks>
        /// <param name="id">Customer id</param>
        /// <param name="customer">customer</param>
        /// <returns>Customer</returns>
        [HttpPut("{id}/UpdateCustomer")]
        public async Task<ActionResult<CustomerDto>> UpdateCustomer(int id, [FromBody] CustomerDto customer)
        {
            if (customer == null)
            {
                return BadRequest("Customer data is required.");
            }

            try
            {
                var updatedCustomer = await _customerService.UpdateCustomerAsync(id, customer);
                return Ok(updatedCustomer);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Endpoint to delete an existing customer
        /// </summary>
        /// <remarks>This is to delete an existing customer.</remarks>
        /// <param name="id">Customer id</param>
        /// <returns>Deleted status</returns>
        /// <response code="204">Return 204 No Content if deletion is successful</response>
        [HttpDelete("{id}/DeleteCustomer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                await _customerService.DeleteCustomerAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}