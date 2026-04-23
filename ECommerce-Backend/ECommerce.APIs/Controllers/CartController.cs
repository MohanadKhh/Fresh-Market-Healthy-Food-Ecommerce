using ECommerce.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartManager _cartManager;
        public CartController(ICartManager cartManager)
        {
            _cartManager = cartManager;
        }

        [HttpGet]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<IActionResult> GetCartAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _cartManager.GetCartByUserIdAsync(userId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPost]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<IActionResult> AddCartItemAsync([FromBody] CartItemWriteDto cartItemWriteDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(); 
            
            var result = await _cartManager.AddItemAsync(userId, cartItemWriteDto);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpDelete("{productId:int}")]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<IActionResult> RemoveCartItemAsync([FromRoute] int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _cartManager.RemoveItemAsync(userId, productId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPut]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<IActionResult> UpdateCartItemAsync([FromBody] CartItemWriteDto cartItemWriteDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _cartManager.EditCartItemQuantityAsync(userId, cartItemWriteDto);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}
