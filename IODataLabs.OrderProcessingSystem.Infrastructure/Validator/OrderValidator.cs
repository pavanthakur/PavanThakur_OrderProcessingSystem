﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IODataLabs.OrderProcessingSystem.Infrastructure.Validator
{
    using FluentValidation;
    using IODataLabs.OrderProcessingSystem.Infrastructure.DataContext;
    using IODataLabs.OrderProcessingSystem.Infrastructure.Entities;
    using Microsoft.EntityFrameworkCore;

    public class OrderValidator : AbstractValidator<Order>
    {
        private readonly OrderProcessingSystemDbContext _context;

        public OrderValidator(OrderProcessingSystemDbContext context)
        {
            _context = context;

            RuleFor(x => x.CustomerId).MustAsync(HasUnfulfilledPreviousOrder).WithMessage("Customer has an unfulfilled previous order.");
        }

        private async Task<bool> HasUnfulfilledPreviousOrder(int customerId, CancellationToken cancellationToken)
        {
            return await _context.Orders
                .Where(o => o.CustomerId == customerId && !o.IsFulfilled)
                .AnyAsync(cancellationToken);
        }
    }

}