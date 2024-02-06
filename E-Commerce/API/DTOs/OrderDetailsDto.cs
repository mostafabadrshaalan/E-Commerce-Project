namespace API.DTOs
{
    public record OrderDetailsDto
    {
        public int Id { get; set; }
        public IReadOnlyList<OrderItemDto> OrderItems { get; set; }
        public string CustomerEmail { get; set; }
        public string DeliveryMethod { get; set; }
        public ShippingAddressDto ShippingAddress { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public decimal SubTotal { get; set; }
        public string PaymentIntentId { get; set; }
        public decimal Total { get; set; }
        public decimal ShippingPrice { get; set; }
        public string OrderStatus { get; set; }

    }
}
