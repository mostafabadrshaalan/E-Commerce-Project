using Core.Entities.OrderAggergate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string customerEmail,
                                     int deliveryMethodId,
                                     string basketId,
                                     ShippingAddress shippingAddress);

        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string customerEmail);

        Task<Order> GetOrderByIdAsync(int orderId, string customerEmail);

        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
    }
}
