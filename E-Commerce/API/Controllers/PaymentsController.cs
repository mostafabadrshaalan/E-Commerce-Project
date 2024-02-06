using API.ResponseModule;
using Core.Entities;
using Core.Entities.OrderAggergate;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Stripe;


namespace API.Controllers
{
    public class PaymentsController : BaseController
    {
        private readonly IPaymentService paymentService;
        private readonly ILogger<PaymentsController> logger;
        private const string WhSecret = "";

        public PaymentsController(IPaymentService paymentService, ILogger<PaymentsController> logger)
        {
            this.paymentService = paymentService;
            this.logger = logger;
        }

        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (basket == null)
                return BadRequest(new ApiResponse(400, "There is problem with your basket"));
            return Ok(basket);
        }

        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], WhSecret);
            PaymentIntent paymentIntent;
            Order order;
            switch (stripeEvent.Type)
            {
                case Events.PaymentIntentPaymentFailed:
                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    logger.LogInformation("Payment Failed : ", paymentIntent.Id);
                    order = await paymentService.UpdateOrderPaymentFailed(paymentIntent.Id);
                    logger.LogInformation("Payment Failed : ", order.Id);
                    break;
                case Events.PaymentIntentSucceeded:
                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    logger.LogInformation("Payment Succeeded : ", paymentIntent.Id);
                    order = await paymentService.UpdateOrderPaymentSucceeded(paymentIntent.Id);
                    logger.LogInformation("Order updated to payment recieved : ", order.Id);
                    break;
            }
            return new EmptyResult();
        }

    }
}
