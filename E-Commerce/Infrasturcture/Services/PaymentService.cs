using Core.Entities;
using Core.Entities.OrderAggergate;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.Extensions.Configuration;
using Stripe;
using Product = Core.Entities.Product;

namespace Infrasturcture.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration configuration;
        private readonly IBasketRepository basketRepository;
        private readonly IUnitOfWork unitOfWork;

        public PaymentService(IConfiguration configuration, IBasketRepository basketRepository, IUnitOfWork unitOfWork)
        {
            this.configuration = configuration;
            this.basketRepository = basketRepository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = configuration["StripeSettings:SecretKey"];

            var basket = await basketRepository.GetBasketAsync(basketId);

            if (basket is null)
                return null;

            var shippingPrice = 0m;
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliverMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                shippingPrice = deliverMethod.Price;
            }

            foreach (var item in basket.BasketItems)
            {
                var product = await unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

                item.Price = product.Price;
            }

            var service = new PaymentIntentService();
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(basket.BasketItems.Sum(item => item.Quentity * item.Price * 100) + (shippingPrice * 100)),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };

                var intent = await service.CreateAsync(options);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)(basket.BasketItems.Sum(item => item.Quentity * item.Price * 100) + (shippingPrice * 100)),
                };
                await service.UpdateAsync(basket.PaymentIntentId, options);
            }

            await basketRepository.UpdateBasketAsync(basket);
            return basket;
        }

        public async Task<Order> UpdateOrderPaymentFailed(string paymentIntentId)
        {
            return await OrderStatusComplete(OrderStatus.PaymentFailed, paymentIntentId);
        }

        public async Task<Order> UpdateOrderPaymentSucceeded(string paymentIntentId)
        {
            return await OrderStatusComplete(OrderStatus.PaymentSucceeded, paymentIntentId);
        }

        private async Task<Order> OrderStatusComplete(OrderStatus orderStatus, string paymentIntentId)
        {
            var specs = new OrderWithPaymentIntentSpecifications(paymentIntentId);

            var order = await unitOfWork.Repository<Order>().GetEntityWithSpecitfications(specs);

            if (order is null)
                return null;

            order.OrderStatus = orderStatus;

            unitOfWork.Repository<Order>().Update(order);

            await unitOfWork.Complete();

            return order;
        }
    }
}
