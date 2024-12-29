using IODataLabs.OrderProcessingSystem.Application.DTO;
using IODataLabs.OrderProcessingSystem.Application.Interfaces;
using IODataLabs.OrderProcessingSystem.Application.Request;
using IODataLabs.OrderProcessingSystem.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
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
        [HttpPost]
        public async Task<ActionResult> CreateCustomer(CreateCustomerRequest customer)
        {
            int customerId = await _customerService.CreateCustomerAsync(customer);
            if (customerId > 0)
                return CreatedAtAction(nameof(CreateCustomer), new { id = customerId });
            return StatusCode(500);
        }
    }
}