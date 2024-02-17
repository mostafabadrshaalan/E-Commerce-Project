using Core.Entities;
using Core.Entities.OrderAggergate;
using Core.Interfaces;
using Core.Specifications;

namespace Infrasturcture.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IBasketRepository basketRepository;
        private readonly IPaymentService paymentService;

        public OrderService(IUnitOfWork unitOfWork, IBasketRepository basketRepository, IPaymentService paymentService)
        {
            this.unitOfWork = unitOfWork;
            this.basketRepository = basketRepository;
            this.paymentService = paymentService;
        }

        public async Task<Order> CreateOrderAsync(string customerEmail, int deliveryMethodId, string basketId, ShippingAddress shippingAddress)
        {
            // Get Basket
            var basket = await basketRepository.GetBasketAsync(basketId);

            // Get Basket Items
            var orderItems = await GetBasketItems(basket);

            // Calculate Subtotal
            var subtotal = orderItems.Sum(item => item.Price * item.Qantity);

            // Get DeliveryMethod
            var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            //TODO Payment Process
            //Check if the order exists
            var specs = new OrderWithPaymentIntentSpecifications(basket.PaymentIntentId);

            var existingOrder = await unitOfWork.Repository<Order>().GetEntityWithSpecitfications(specs);

            if (existingOrder != null)
            {
                unitOfWork.Repository<Order>().Delete(existingOrder);
                await paymentService.CreateOrUpdatePaymentIntent(basketId);
            }

            // Create Order
            var order = new Order(customerEmail, deliveryMethod, shippingAddress, subtotal, orderItems, basket.PaymentIntentId);

            unitOfWork.Repository<Order>().Add(order);
            int result = 0;
            try
            {
                result = await unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


            if (result <= 0)
                return null;

            // Delete Basket
            //await basketRepository.DeleteBasketAsync(basketId);

            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
            => await unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

        public Task<Order> GetOrderByIdAsync(int orderId, string customerEmail)
        {
            var orderSpecs = new OrderWithItemsSpecifications(orderId, customerEmail);

            return unitOfWork.Repository<Order>().GetEntityWithSpecitfications(orderSpecs);
        }

        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string customerEmail)
        {
            var orderSpecs = new OrderWithItemsSpecifications(customerEmail);

            return unitOfWork.Repository<Order>().GetAllWithSpecifications(orderSpecs);
        }

        private async Task<List<OrderItem>> GetBasketItems(CustomerBasket customerBasket)
        {
            var orderItems = new List<OrderItem>();
            foreach (var basketItem in customerBasket.BasketItems)
            {
                var product = await unitOfWork.Repository<Product>().GetByIdAsync(basketItem.Id);

                var productItem = new ProductItem(product.Id, product.Name, product.PictureUrl);

                var orderItem = new OrderItem(productItem, product.Price, basketItem.Quentity);

                orderItems.Add(orderItem);
            }
            return orderItems;
        }
    }
}
