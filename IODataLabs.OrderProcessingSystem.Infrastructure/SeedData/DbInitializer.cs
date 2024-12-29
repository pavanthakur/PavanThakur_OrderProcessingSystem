﻿using IODataLabs.OrderProcessingSystem.Domain.Entities;
using IODataLabs.OrderProcessingSystem.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IODataLabs.OrderProcessingSystem.Infrastructure.SeedData
{
    public static class DbInitializer
    {
        public static void Initialize(OrderProcessingSystemDbContext context)
        {
            context.Database.EnsureCreated();  // Ensure the DB is created

            // Check if the data already exists
            if (context.Customers.Any() || context.Products.Any() || context.Orders.Any() || context.OrderProducts.Any())
            {
                return; // DB has been seeded
            }

            // Seed Customers
            var customers = new List<Customer>
            {
                new Customer { Name = "John Doe", Email = "john.doe@example.com" },
                new Customer { Name = "Jane Smith", Email = "jane.smith@example.com" }
            };
            context.Customers.AddRange(customers);
            context.SaveChanges();

            // Seed Products
            var products = new List<Product>
            {
                new Product { Name = "Laptop", Price = 500.00m },
                new Product { Name = "Phone", Price = 300.00m },
                new Product { Name = "Headphones", Price = 200.00m }
            };
            context.Products.AddRange(products);
            context.SaveChanges();

            // Seed Orders
            var orders = new List<Order>
            {
                new Order { OrderDate = DateTime.Now, CustomerId = customers[0].CustomerId, TotalPrice =  products.Sum(product => product.Price)},
                new Order { OrderDate = DateTime.Now, CustomerId = customers[1].CustomerId }
            };
            context.Orders.AddRange(orders);
            context.SaveChanges();

            // Seed OrderProducts (Many-to-many relationship)
            var orderProducts = new List<OrderProduct>
            {
                new OrderProduct { OrderId = orders[0].OrderId, ProductId = products[0].ProductId, Quantity = 1 },
                new OrderProduct { OrderId = orders[0].OrderId, ProductId = products[1].ProductId, Quantity = 2 },
                new OrderProduct { OrderId = orders[1].OrderId, ProductId = products[2].ProductId, Quantity = 1 }
            };
            context.OrderProducts.AddRange(orderProducts);
            context.SaveChanges();

            UpdateOrderTotalPrices(context);
        }

        private static void UpdateOrderTotalPrices(OrderProcessingSystemDbContext context)
        {
            // Perform the LINQ query to calculate the total price for each order
            var orderTotalPrices = context.Orders
                .Join(context.OrderProducts,
                    o => o.OrderId,
                    op => op.OrderId,
                    (o, op) => new { o, op })
                .Join(context.Products,
                    o_op => o_op.op.ProductId,
                    p => p.ProductId,
                    (o_op, p) => new { o_op.o, TotalPrice = p.Price * o_op.op.Quantity })
                .GroupBy(o_p => o_p.o.OrderId)
                .Select(g => new
                {
                    OrderId = g.Key,
                    TotalPrice = g.Sum(x => x.TotalPrice) // Sum the prices of the matched products
                })
                .ToList();

            // Update each order with the calculated total price
            foreach (var orderTotal in orderTotalPrices)
            {
                var order = context.Orders.Find(orderTotal.OrderId);
                if (order != null)
                {
                    order.TotalPrice = orderTotal.TotalPrice;
                }
            }
            // Save the changes to the database
            context.SaveChanges();
        }
    }
}