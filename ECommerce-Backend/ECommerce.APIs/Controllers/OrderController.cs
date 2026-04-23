using ECommerce.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderManager _orderManager;

        public OrderController(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        [HttpGet]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<IActionResult> GetOrdersOfUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _orderManager.GetAllOrdersByUserIdAsync(userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{orderId:int}")]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<IActionResult> GetOrderById([FromRoute] int orderId)
        {
            var result = await _orderManager.GetOrderByIdAsync(orderId);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderWriteDto orderWriteDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _orderManager.PlaceOrderAsync(userId, orderWriteDto);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
