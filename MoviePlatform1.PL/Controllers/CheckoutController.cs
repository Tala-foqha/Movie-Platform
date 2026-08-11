using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviePlatform1.BLL.Services;
using MoviePlatform1.DAL.Dto.Request;


namespace MoviePlatform1.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        // المستخدم يضغط Checkout
        [HttpPost]
        public async Task<IActionResult> ProcessCheckout(
            CheckoutRequest request)
        {
            var userId = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();

            var result = await _checkoutService.processCheckout(
                userId,
                request);

            return Ok(result);
        }

        // Stripe يرجع المستخدم هون بعد نجاح الدفع
        [AllowAnonymous]
        [HttpGet("Success")]
        public async Task<IActionResult> Success(
            [FromQuery] string sessionId)
        {
            var result = await _checkoutService.HandleSuccess(sessionId);

            return Ok(result);
        }

        // Stripe يرجع المستخدم هون إذا ألغى عملية الدفع
        [AllowAnonymous]
        [HttpGet("cancel")]
        public IActionResult Cancel()
        {
            return Ok(new
            {
                Success = false,
                Message = "Payment was cancelled."
            });
        }
    }
}