using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviePlatform1.BLL.Services;
using MoviePlatform1.DAL.Dto.Request;

namespace MoviePlatform1.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService; 
        }
        [HttpPost("register")]
      
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request is null");
            }

            var result = await _authenticationService.RegisterAsync(request);

            return Ok(result);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request is null");
            }

            var result = await _authenticationService.LoginAsync(request);

            return Ok(result);
        }
   
        [HttpGet("confirmemail")]
        public async Task<IActionResult> ConfirmEmail(string token, string userId)
        {
            bool isConfirmed = await _authenticationService.confirmEmailAsync(token, userId);

            if (isConfirmed)
            {
                return Content(
                    "<h1>Email Confirmed Successfully ✅</h1>",
                    "text/html"
                );
            }

            return Content(
                "<h1>Email Confirmation Failed ❌</h1>",
                "text/html"
            );
        }
    }
}
