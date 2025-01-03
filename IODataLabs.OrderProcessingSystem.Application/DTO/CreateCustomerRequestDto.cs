using System;
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
        public string Name { get; set; }

        [Required]

        public string Email { get; set; }

    }
}
