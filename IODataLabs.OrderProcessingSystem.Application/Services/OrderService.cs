using FluentValidation;
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
    public class OrderService : IOrderService
    {
        private readonly OrderProcessingSystemDbContext _context;
        private readonly IValidator<Order> _orderValidator;


        public OrderService(OrderProcessingSystemDbContext context, IValidator<Order> orderValidator)
        {
            _context = context;
            _orderValidator = orderValidator;
        }

        // Create a new order
        public async Task<Order> CreateOrderAsync(int customerId, List<int> productIds)
        {
            var customer = await _context.Customers.Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
            if (customer == null)
                throw new KeyNotFoundException($"Customer with ID {customerId} not found.");

            // Validate customer order history: Check if there is an unfulfilled order
            if (customer.Orders.Any(o => !o.IsFulfilled))
                throw new ValidationException("Customer cannot place a new order until their previous order is fulfilled.");

            // Retrieve the products and check if they exist
            var products = await _context.Products.Where(p => productIds.Contains(p.ProductId)).ToListAsync();
            if (products.Count != productIds.Count)
                throw new KeyNotFoundException("One or more products not found.");

            // Create new order
            var order = new Order
            {
                CustomerId = customerId,
                OrderProducts = products.Select(p => new OrderProduct
                {
                    ProductId = p.ProductId,
                    Product = p,
                    Quantity = 1  // Assuming quantity of 1 for simplicity; you can modify this to accept quantity
                    //Price = p.Price
                }).ToList()
            };

            // Calculate the total price
            order.TotalPrice = order.OrderProducts.Sum(oi => oi.Quantity * oi.Price);

            // Validate the order using FluentValidation
            await _orderValidator.ValidateAndThrowAsync(order);

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        // Retrieve an order by ID, including the total price
        public async Task<Order> GetOrderDetailsAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
                throw new KeyNotFoundException($"Order with ID {orderId} not found.");

            // Calculate the total price
            order.TotalPrice = order.OrderProducts.Sum(oi => oi.Quantity * oi.Price);
            return order;
        }

        // Update an order (for simplicity, we are just updating its fulfillment status here)
        public async Task<Order> UpdateOrderAsync(int orderId, bool isFulfilled)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {orderId} not found.");

            order.IsFulfilled = isFulfilled;
            await _context.SaveChangesAsync();
            return order;
        }

        // Delete an order
        public async Task DeleteOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {orderId} not found.");

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}
