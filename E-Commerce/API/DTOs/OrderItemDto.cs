namespace API.DTOs
{
    public record OrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int Qantity { get; set; }
    }
}