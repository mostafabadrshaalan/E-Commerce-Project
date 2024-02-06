using API.DTOs;
using API.ResponseModule;
using AutoMapper;
using Core.Entities.OrderAggergate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }

        [HttpPost("CreateOrder")]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var shippingAddress = mapper.Map<ShippingAddress>(orderDto.ShippingAddress);

            var order = await orderService.CreateOrderAsync(email, orderDto.DeliveryMethodId, orderDto.BasketId, shippingAddress);

            if (order is null)
                return BadRequest(new ApiResponse(400, "Order is not valid"));

            return Ok(order);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetailsDto>> GetOrderById(int id)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var order = await orderService.GetOrderByIdAsync(id, email);

            if (order is null)
                return NotFound(new ApiResponse(404, "Order is not found"));

            var mappedOrder = mapper.Map<OrderDetailsDto>(order);
            return Ok(mappedOrder);
        }

        [HttpGet("GetAllOrderForUser")]
        public async Task<ActionResult<OrderDetailsDto>> GetOrdersForUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var orders = await orderService.GetOrdersForUserAsync(email);

            var mappedOrders = mapper.Map<IReadOnlyList<OrderDetailsDto>>(orders);
            return Ok(mappedOrders);
        }

        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> DeliverMethods()
        {
            var deliverMethods = await orderService.GetDeliveryMethodsAsync();

            return Ok(deliverMethods);
        }
    }
}
