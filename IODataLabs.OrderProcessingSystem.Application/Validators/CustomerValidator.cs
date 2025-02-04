﻿using FluentValidation;
using IODataLabs.OrderProcessingSystem.Application.DTO;

namespace IODataLabs.OrderProcessingSystem.Application.Validators
{
    public class CustomerValidator : AbstractValidator<CustomerDto>
    {
        public CustomerValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.")
                                  .EmailAddress().WithMessage("Invalid email format.");
        }
    }

}
