using IODataLabs.OrderProcessingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IODataLabs.OrderProcessingSystem.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(int customerId, List<int> productIds);
        Task DeleteOrderAsync(int orderId);
        Task<Order> GetOrderDetailsAsync(int orderId);
        Task<Order> UpdateOrderAsync(int orderId, bool isFulfilled);
    }

}
