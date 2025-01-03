using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IODataLabs.OrderProcessingSystem.Domain.Entities
{
    public class Customer : BaseAuditableEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();
    }

}
