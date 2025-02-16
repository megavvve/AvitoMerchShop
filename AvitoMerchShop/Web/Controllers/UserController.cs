using System.Security.Claims;
using AvitoMerchShop.Application.Requests;
using AvitoMerchShop.Application.Responses;
using AvitoMerchShop.Core.Interfaces;
using AvitoMerchShop.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace AvitoMerchShop.Controllers
{
    [ApiController]
    [Route("api")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> TransferCoins([FromBody] CoinTransactionRequest request)
        {
            var success = await _userService.TransferCoins(request.FromUserId, request.ToUserId, request.Amount);
            if (!success)
                return BadRequest(new { message = "Invalid transaction" });

            return Ok(new { message = "Coins transferred successfully" });
        }

      
        [HttpPost("purchase")]
        public async Task<IActionResult> PurchaseItem([FromBody] PurchaseRequest request)
        {
            var success = await _userService.PurchaseItem(request.UserId, request.ItemId, request.Quantity);
            if (!success)
                return BadRequest(new { message = "Not enough coins or invalid item" });

            return Ok(new { message = "Purchase successful" });
        }

        [Authorize]
        [HttpGet("buy/{item}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        [ProducesResponseType(401, Type = typeof(ErrorResponse))]
        [ProducesResponseType(500, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> BuyItem(string item)
        {
            try
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!int.TryParse(userIdString, out int userId))
                    return Unauthorized(new ErrorResponse { Errors = "Invalid token" });

                var success = await _userService.PurchaseItemByName(userId, item);
                if (!success)
                    return BadRequest(new ErrorResponse { Errors = "Not enough coins or item not found" });

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error buying item: {ex}");
                return StatusCode(500, new ErrorResponse { Errors = "Internal server error" });
            }
        }



    }
}