using IODataLabs.OrderProcessingSystem.Application.DTO;
using IODataLabs.OrderProcessingSystem.Application.Interfaces;
using IODataLabs.OrderProcessingSystem.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using AutoMapper;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace IODataLabs.OrderProcessingSystem.UnitTest.ControllersTest
{
    public class CustomersControllerTest
    {
        private readonly Mock<ICustomerService> _mockCustomerService;
        private readonly CustomerController _controller;

        public CustomersControllerTest()
        {
            _mockCustomerService = new Mock<ICustomerService>();
            _controller = new CustomerController(_mockCustomerService.Object);
        }

        [Fact]
        public async Task GetAllCustomers_ReturnsOkResult_WithListOfCustomers()
        {
            // Arrange
            var customers = new List<CustomerDto> { new CustomerDto { CustomerId = 1, Name = "John Doe", Email = "test@test1.com" } };
            _mockCustomerService.Setup(service => service.GetAllCustomersAsync()).ReturnsAsync(customers);

            // Act
            var result = await _controller.GetAllCustomers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<CustomerDto>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public void VerifyExceptionLoggedInService_ReturnsException()
        {
            // Arrange
            _mockCustomerService.Setup(service => service.VerifyExceptionLoggedInService()).Throws(new System.Exception());

            // Act
            var ex = Record.Exception(() => _controller.VerifyExceptionLoggedInService());

            // Assert
            Assert.NotNull(ex);
            Assert.IsType<System.Exception>(ex);
        }

        [Fact]
        public async Task GetCustomerById_ReturnsOkResult_WithCustomer()
        {
            // Arrange
            var customer = new CustomerDto { CustomerId = 1, Name = "John Doe", Email = "test@test1.com" };
            _mockCustomerService.Setup(service => service.GetCustomerWithOrdersAsync(1)).ReturnsAsync(customer);

            // Act
            var result = await _controller.GetCustomerById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<CustomerDto>(okResult.Value);
            Assert.Equal(1, returnValue.CustomerId);
        }

        [Fact]
        public async Task GetCustomerById_ReturnsNotFound_WhenCustomerNotFound()
        {
            // Arrange
            _mockCustomerService.Setup(service => service.GetCustomerWithOrdersAsync(1)).Throws(new KeyNotFoundException("Customer not found"));

            // Act
            var result = await _controller.GetCustomerById(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Customer not found", notFoundResult.Value);
        }

        [Fact]
        public async Task CreateCustomer_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var createCustomerRequest = new CreateCustomerRequestDto { Name = "John Doe", Email = "test@test1.com" };
            _mockCustomerService.Setup(service => service.CreateCustomerAsync(createCustomerRequest)).ReturnsAsync(1);

            // Act
            var result = await _controller.CreateCustomer(createCustomerRequest);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(CustomerController.CreateCustomer), createdAtActionResult.ActionName);
        }

        [Fact]
        public async Task CreateCustomer_ReturnsStatusCode500_WhenCustomerCreationFails()
        {
            // Arrange
            var createCustomerRequest = new CreateCustomerRequestDto { Name = "John Doe", Email = "test@test1.com" };
            _mockCustomerService.Setup(service => service.CreateCustomerAsync(createCustomerRequest)).ReturnsAsync(0);

            // Act
            var result = await _controller.CreateCustomer(createCustomerRequest);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }
}