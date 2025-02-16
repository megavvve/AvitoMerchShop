using AvitoMerchShop.Application.Requests;
using AvitoMerchShop.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;


namespace AvitoMerchShop.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService userService, ILogger<AuthController> logger)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] AuthRequest request)
        {
            try
            {
                var user = await _userService.Authenticate(request.Username, request.Password);
                if (user == null)
                    return Unauthorized(new { message = "Invalid credentials" });

                var token = _userService.GenerateJwtToken(user);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Authentication error: {ex.Message}");
                return StatusCode(500, new { message = "Internal Server Error" });
            }
        }

    }




}
