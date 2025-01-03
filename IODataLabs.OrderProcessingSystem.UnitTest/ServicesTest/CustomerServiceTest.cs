using FluentAssertions;
using IODataLabs.OrderProcessingSystem.Application.DTO;
using IODataLabs.OrderProcessingSystem.Domain.Entities;
using IODataLabs.OrderProcessingSystem.UnitTest.TestBase;
using Microsoft.Extensions.Logging;
using Moq;


namespace IODataLabs.OrderProcessingSystem.UnitTest.ServicesTest
{
    public class CustomerServiceTests : CustomerServiceTestBase
    {
        public CustomerServiceTests()
        {
        }

        [Fact]
        async Task CreateCustomerAsync_ShouldReturnCustomerId_WhenCustomerIsCreated()
        {
            // Arrange
            var newCustomer = GenerateNewCustomerRequestDto(1)[0];
            var customer = GenerateCustomers(1).FirstOrDefault().As<Customer>;
            MockMapper.Setup(m => m.Map<CreateCustomerRequestDto, Customer>(newCustomer)).Returns(customer);
            MockDbContext.Setup(db => db.Customers.Add(It.IsAny<Customer>()));
            MockDbContext.Setup(db => db.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _customerService.CreateCustomerAsync(newCustomer);

            // Assert
            MockDbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once); // Ensure SaveChanges was called
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task GetAllCustomersAsync_ShouldReturnListOfCustomers()
        {
            // Arrange
            var customers = GenerateCustomers(5).AsQueryable();
            var mockDbSet = GetMockDbSet(customers);
            IEnumerable<CustomerDto> customerDtos = customers.Select(c => new CustomerDto
            {
                CustomerId = c.CustomerId,
                Name = c.Name,
                Email = c.Email
            }).ToList();

            MockMapper.Setup(m => m.Map<IEnumerable<CustomerDto>>(customers)).Returns(customerDtos);
            MockDbContext.Setup(db => db.Customers).Returns(mockDbSet.Object);

            // Act
            var result = await _customerService.GetAllCustomersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count()); // Ensure 5 customers are returned
            Assert.Equal(customers.First().Name, result.First().Name);  // Ensure the first customer's name matches
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ShouldReturnCustomer_WhenCustomerExists()
        {
            // Arrange
            var customer = GenerateCustomers(1).First();
            var customerDto = new CustomerDto
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                Email = customer.Email
            };
            MockMapper.Setup(m => m.Map<CustomerDto>(customer)).Returns(customerDto);
            MockDbContext.Setup(db => db.Customers.FindAsync(1)).ReturnsAsync(customer);

            // Act
            var result = await _customerService.GetCustomerByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customer.CustomerId, result.CustomerId);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            MockDbContext.Setup(c => c.Customers.FindAsync(1)).ReturnsAsync((Customer?)null);

            // Act
            var result = await _customerService.GetCustomerByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void VerifyExceptionLoggedInService_ShouldLogException()
        {
            // Arrange
            MockLogger.Setup(l => l.Log(
                It.Is<LogLevel>(level => level == LogLevel.Error),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()));

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => _customerService.VerifyExceptionLoggedInService());
            Assert.Equal("Test exception to verify ErrorHandlingMiddleware is working", exception.Message);

            MockLogger.Verify(l => l.Log(
                It.Is<LogLevel>(level => level == LogLevel.Error),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()), Times.Once);
        }

        [Fact]
        public void VerifyExceptionLoggedInService_ReturnsException()
        {
            // Arrange
            MockLogger.Setup(l => l.Log(
                It.Is<LogLevel>(level => level == LogLevel.Error),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()));

            // Act
            var ex = Record.Exception(() => _customerService.VerifyExceptionLoggedInService());

            // Assert
            Assert.NotNull(ex);
            Assert.IsType<Exception>(ex);
        }

        [Fact]
        public async Task GetCustomerWithOrdersAsync_ShouldReturnCustomerWithOrders_WhenCustomerExists()
        {
            // Arrange
            var customerId = 1;
            var customers = GenerateCustomersWithOrders(1, 1).AsQueryable();
            customers.First().CustomerId = customerId;
            var mockDbSet = GetMockDbSet(customers);

            var customerDto = new CustomerDto
            {
                CustomerId = customers.First().CustomerId,
                Name = customers.First().Name,
                Email = customers.First().Email,
                OrderDtos = customers.First().Orders.Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    CustomerId = customerId,
                    TotalPrice = o.TotalPrice
                }).ToList()
            };

            MockDbContext.Setup(db => db.Customers).Returns(mockDbSet.Object);
            MockMapper.Setup(m => m.Map<CustomerDto>(It.IsAny<Customer>())).Returns(customerDto);

            // Act
            var result = await _customerService.GetCustomerWithOrdersAsync(customerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.CustomerId);
            Assert.Single(result.OrderDtos);
        }
    }
}
