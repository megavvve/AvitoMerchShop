
using Microsoft.AspNetCore.Mvc;
using AvitoMerchShop.Data;
using AvitoMerchShop.Models;
using Microsoft.EntityFrameworkCore;
namespace AvitoMerchShop.Controllers

{

    namespace AvitoMerchShop.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class PurchaseController : ControllerBase
        {
            private readonly AppDbContext _context;

            public PurchaseController(AppDbContext context)
            {
                _context = context;
            }

            [HttpPost("buy")]
            public async Task<IActionResult> BuyItem([FromBody] Purchase request)
            {
                var user = await _context.Users.FindAsync(request.UserId);
                var item = await _context.Items.FindAsync(request.ItemId);

                if (user == null || item == null)
                {
                    return NotFound("User or Item not found");
                }

                int totalCost = item.Price * request.Quantity;

                if (user.CoinsBalance < totalCost)
                {
                    return BadRequest("Not enough coins");
                }

                user.CoinsBalance -= totalCost;

                var purchase = new Purchase
                {
                    UserId = user.Id,
                    ItemId = item.Id,
                    Quantity = request.Quantity
                };

                _context.Purchases.Add(purchase);
                await _context.SaveChangesAsync();

                return Ok(new { user.CoinsBalance, purchase });
            }
        }

      
    }

}
