using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AvitoMerchShop.Models;
using AvitoMerchShop.Services;

namespace AvitoMerchShop.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public ApiController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet("info")]
        [Authorize]
        public IActionResult GetInfo()
        {
            var info = new InfoResponse
            {
                Coins = 1000,  
                Inventory = new List<ItemInventory>
                {
                    new ItemInventory { Type = "T-shirt", Quantity = 5 }
                },
                CoinHistory = new CoinHistory
                {
                    Received = new List<CoinTransaction>
                    {
                        new CoinTransaction { FromUserId=1, Amount = 100 }
                    },
                    Sent = new List<CoinTransaction>
                    {
                        new CoinTransaction { ToUserId=2 , Amount = 50 }
                    }
                }
            };
            return Ok(info);
        }

        [HttpPost("sendCoin")]
        [Authorize]
        public IActionResult SendCoin([FromBody] SendCoinRequest request)
        {
            return Ok();
        }

        [HttpGet("buy/{item}")]
        [Authorize]
        public IActionResult BuyItem(string item)
        {
            return Ok();
        }

        [HttpPost("auth")]
        public IActionResult Authenticate([FromBody] AuthRequest request)
        {
            if (request.Username == "admin" && request.Password == "password") 
            {
                var token = _tokenService.GenerateToken(request.Username);  
                return Ok(new AuthResponse { Token = token });
            }
            return Unauthorized(new ErrorResponse { Errors = "Invalid credentials" });
        }
    }
}
