﻿using AutoMapper;
using IODataLabs.OrderProcessingSystem.Application.DTO;
using IODataLabs.OrderProcessingSystem.Application.Interfaces;
using IODataLabs.OrderProcessingSystem.Domain.Entities;
using IODataLabs.OrderProcessingSystem.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IODataLabs.OrderProcessingSystem.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly OrderProcessingSystemDbContext _context;
        private readonly ILogger<CustomerService> _logger;
        private readonly IMapper _autoMapper;

        public CustomerService(OrderProcessingSystemDbContext context, ILogger<CustomerService> logger, IMapper autoMapper)
        {
            _context = context;
            _logger = logger;
            _autoMapper = autoMapper;
        }

        // Retrieve all customers
        public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
        {
            var customers = await _context.Customers.ToListAsync();
            return _autoMapper.Map<IEnumerable<CustomerDto>>(customers);
        }

        // Get Customer by Id
        public async Task<CustomerDto> GetCustomerByIdAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return null;
            }
            return _autoMapper.Map<CustomerDto>(customer);
        }

        // Verify Exception Logged In Service
        public void VerifyExceptionLoggedInService()
        {
            _logger.LogInformation("Executing VerifyExceptionLoggedInService...");
            try
            {
                // Your business logic
                throw new Exception("Test exception to verify ErrorHandlingMiddleware is working");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing VerifyExceptionLoggedInService.");
                throw;  // Re-throw the exception after logging
            }
        }

        // Retrieve a specific customer with their orders
        public async Task<CustomerDto> GetCustomerWithOrdersAsync(int customerId)
        {
            var customerDto = await _context.Customers
                .Include(c => c.Orders)
                .ThenInclude(o => o.OrderProducts)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (customerDto == null)
                throw new KeyNotFoundException($"Customer with ID {customerId} not found.");

            return _autoMapper.Map<CustomerDto>(customerDto);
        }

        // Create a new customer
        public async Task<CustomerDto> CreateCustomerAsync(CustomerDto customerDto)
        {
            var customer = _autoMapper.Map<Customer>(customerDto);
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return _autoMapper.Map<CustomerDto>(customer);
        }

        // Update an existing customer
        public async Task<CustomerDto> UpdateCustomerAsync(int customerId, CustomerDto updatedCustomerDto)
        {
            var existingCustomer = await _context.Customers.FindAsync(customerId);
            if (existingCustomer == null)
                throw new KeyNotFoundException($"Customer with ID {customerId} not found.");

            _autoMapper.Map(updatedCustomerDto, existingCustomer);
            _context.Customers.Update(existingCustomer);
            await _context.SaveChangesAsync();

            return _autoMapper.Map<CustomerDto>(existingCustomer);
        }

        // Delete a customer
        public async Task DeleteCustomerAsync(int customerId)
        {
            var customerDto = await _context.Customers.FindAsync(customerId);
            if (customerDto == null)
                throw new KeyNotFoundException($"Customer with ID {customerId} not found.");

            _context.Customers.Remove(customerDto);
            await _context.SaveChangesAsync();
        }
    }
}
