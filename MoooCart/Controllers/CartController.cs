using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoooCart.lib.Base;
using MoooCart.lib.DTOs;
using MoooCart.Services;
using System.Security.Claims;

namespace MoooCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController(ICartService service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var cart = await service.GetCartAsync(userId);
            return Ok(cart);
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItem(DtoCartItemRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var result = await service.AddItemToCartAsync(request, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("items/{id}")]
        public async Task<IActionResult> UpdateItemQuantity(Guid id, [FromBody] int quantity)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            if (quantity <= 0) return BadRequest(new DtoResponse(false, "Quantity must be greater than zero."));

            var result = await service.UpdateItemQuantityAsync(id, quantity, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("items/{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var result = await service.DeleteItemFromCartAsync(id, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var result = await service.DoCheckout(userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("my-orders")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token.");

            var history = await service.GetUserOrderHistory(userId);
            return Ok(history);
        }

        [HttpGet("all-orders")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            var history = await service.GetAllOrdersAsync();
            return Ok(history);
        }
    }
}