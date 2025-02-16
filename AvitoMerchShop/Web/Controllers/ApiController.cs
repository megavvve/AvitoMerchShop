using AvitoMerchShop.Application.Responses;
using AvitoMerchShop.Core.Entities;
using AvitoMerchShop.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AvitoMerchShop.Controllers
{
    [ApiController]
    [Route("api")]
    public class ApiController : ControllerBase
    {
        private readonly IUserService _userService;

        public ApiController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("info")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(InfoResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> GetInfo()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
       
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                Console.WriteLine($"Invalid user ID in token: {userIdString}");
                return Unauthorized(new { message = "Invalid token structure" });
            }

            try
            {
                var info = await _userService.GetUserInfo(userId);
                return Ok(info);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user info: {ex}");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

    }
}