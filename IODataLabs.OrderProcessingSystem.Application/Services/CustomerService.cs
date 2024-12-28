using IODataLabs.OrderProcessingSystem.Domain.Entities;
using IODataLabs.OrderProcessingSystem.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IODataLabs.OrderProcessingSystem.Application.Services
{
    public class CustomerService
    {
        private readonly OrderProcessingSystemDbContext _context;

        public CustomerService(OrderProcessingSystemDbContext context)
        {
            _context = context;
        }

        // Retrieve all customers
        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        // Retrieve a specific customer with their orders
        public async Task<Customer> GetCustomerWithOrdersAsync(int customerId)
        {
            var customer = await _context.Customers
                .Include(c => c.Orders)
                .ThenInclude(o => o.OrderProducts)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (customer == null)
                throw new KeyNotFoundException($"Customer with ID {customerId} not found.");

            return customer;
        }

        // Create a new customer
        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        // Update an existing customer
        public async Task<Customer> UpdateCustomerAsync(int customerId, Customer updatedCustomer)
        {
            var existingCustomer = await _context.Customers.FindAsync(customerId);
            if (existingCustomer == null)
                throw new KeyNotFoundException($"Customer with ID {customerId} not found.");

            existingCustomer.Name = updatedCustomer.Name;
            existingCustomer.Email = updatedCustomer.Email;

            await _context.SaveChangesAsync();
            return existingCustomer;
        }

        // Delete a customer
        public async Task DeleteCustomerAsync(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null)
                throw new KeyNotFoundException($"Customer with ID {customerId} not found.");

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }
    }
}
