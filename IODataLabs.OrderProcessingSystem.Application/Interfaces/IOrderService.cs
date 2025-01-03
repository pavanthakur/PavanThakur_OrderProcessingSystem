using IODataLabs.OrderProcessingSystem.Application.DTO;
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
        Task<OrderDto> CreateOrderAsync(int customerId, List<int> productIds);
        Task<OrderDto> GetOrderDetailsAsync(int orderId);
    }

}
