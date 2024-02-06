namespace Core.Entities.OrderAggergate
{
    public class OrderItem : BaseEntity
    {
        public OrderItem()
        {

        }
        public OrderItem(ProductItem productItem, decimal price, int qantity)
        {
            ProductItem = productItem;
            Price = price;
            Qantity = qantity;
        }

        public int Id { get; set; }
        public ProductItem ProductItem { get; set; }
        public decimal Price { get; set; }
        public int Qantity { get; set; }
    }
}