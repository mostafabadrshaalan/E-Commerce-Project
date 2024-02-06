using Core.Entities.OrderAggergate;

namespace Core.Specifications
{
    public class OrderWithItemsSpecifications : BaseSpecifications<Order>
    {
        public OrderWithItemsSpecifications(string email) : base(order => order.CustomerEmail == email)
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);
            AddOrderByDescending(order => order.OrderDate);
        }

        public OrderWithItemsSpecifications(int id, string email)
                           : base(order => order.CustomerEmail == email && order.Id == id)
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);
        }
    }
}
