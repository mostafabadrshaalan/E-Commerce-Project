namespace Core.Entities
{
    public class BasketItem
    {
        public int Id { get; set; }
        public int Quentity { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public string Product { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
    }
}