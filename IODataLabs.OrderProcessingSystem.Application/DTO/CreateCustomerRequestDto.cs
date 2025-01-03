﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IODataLabs.OrderProcessingSystem.Application.DTO
{
    public class CreateCustomerRequestDto
    {
        [Required]
        public required string Name { get; set; }

        [Required]

        public required string Email { get; set; }

    }
}