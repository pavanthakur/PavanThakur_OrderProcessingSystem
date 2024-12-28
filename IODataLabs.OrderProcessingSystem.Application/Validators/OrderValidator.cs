using FluentValidation;
using IODataLabs.OrderProcessingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IODataLabs.OrderProcessingSystem.Application.Validators
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(o => o.TotalPrice)
                .GreaterThan(0).WithMessage("The total price must be greater than zero.");

            RuleForEach(o => o.OrderProducts)
                .ChildRules(orderItem =>
                {
                    orderItem.RuleFor(op => op.Quantity)
                        .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
                    orderItem.RuleFor(op => op.Price)
                        .GreaterThan(0).WithMessage("Price must be greater than zero.");
                });
        }
    }
}

