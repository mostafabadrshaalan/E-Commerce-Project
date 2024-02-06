namespace Core.Entities.OrderAggergate
{
    public class Order : BaseEntity
    {
        public Order()
        {

        }
        public Order(string customerEmail,
            DeliveryMethod deliveryMethod,
            ShippingAddress shippingAddress,
            decimal subTotal,
            IReadOnlyList<OrderItem> orderItems,
            string paymentIntentId)
        {
            CustomerEmail = customerEmail;
            DeliveryMethod = deliveryMethod;
            ShippingAddress = shippingAddress;
            SubTotal = subTotal;
            OrderItems = orderItems;
            PaymentIntentId = paymentIntentId;
        }

        public int Id { get; set; }
        public string CustomerEmail { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public decimal SubTotal { get; set; }
        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public string? PaymentIntentId { get; set; }
        public decimal GetTotal()
            => SubTotal + DeliveryMethod.Price;

    }
}
