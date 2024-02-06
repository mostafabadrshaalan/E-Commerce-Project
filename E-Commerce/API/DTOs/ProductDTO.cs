using Core.Entities;

namespace API.DTOs
{
    public record ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
    }
}
