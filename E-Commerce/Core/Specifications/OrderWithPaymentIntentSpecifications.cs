using Core.Entities.OrderAggergate;

namespace Core.Specifications
{
    public class OrderWithPaymentIntentSpecifications : BaseSpecifications<Order>
    {
        public OrderWithPaymentIntentSpecifications(string paymentIntentId)
            : base(order => order.PaymentIntentId == paymentIntentId)
        {
        }
    }
}
