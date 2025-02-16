using AvitoMerchShop.Application.Requests;
using AvitoMerchShop.Application.Responses;
using AvitoMerchShop.Core.Entities;
using AvitoMerchShop.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AvitoMerchShop.Controllers
{
    [ApiController]
    [Route("api")]
    public class CoinController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CoinController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("sendCoin")]
        [Authorize]
        public async Task<IActionResult> SendCoins([FromBody] SendCoinRequest request)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var fromUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var toUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.ToUser);

            if (fromUser == null || toUser == null)
                return BadRequest(new ErrorResponse { Errors = "Invalid user(s)" });

            if (fromUser.Id == toUser.Id)
                return BadRequest(new ErrorResponse { Errors = "You cannot send coins to yourself" });

            if (fromUser.CoinsBalance < request.Amount)
                return BadRequest(new ErrorResponse { Errors = "Insufficient funds" });

            fromUser.CoinsBalance -= request.Amount;
            toUser.CoinsBalance += request.Amount;

            var transaction = new CoinTransaction
            {
                Amount = request.Amount,
                FromUserId = fromUser.Id,
                ToUserId = toUser.Id
            };

            _context.CoinTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Coins successfully transferred" });
        }

    }
}