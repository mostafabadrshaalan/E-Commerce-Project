using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public record CustomerBasketDto
    {

        [Required]
        public string Id { get; set; }

        public int? DeliveryMethod { get; set; }
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Shipping price must be greater than zero")]
        public decimal ShippingPrice { get; set; }
        public List<BasketItemDto> BasketItems { get; set; }
        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }
    }
}
