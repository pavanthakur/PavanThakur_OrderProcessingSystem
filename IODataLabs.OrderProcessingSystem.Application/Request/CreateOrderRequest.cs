using IODataLabs.OrderProcessingSystem.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IODataLabs.OrderProcessingSystem.Application.Request
{
    /// <summary>
    /// Request model for creating an order.
    /// </summary>
    public class CreateOrderRequest
    {
        /// <summary>
        /// Gets or sets the customer ID.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the list of product IDs.
        /// </summary>
        public required List<int> ProductIds { get; set; }
    }
}
