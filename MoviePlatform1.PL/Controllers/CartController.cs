using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MoviePlatform1.BLL.Services;
using MoviePlatform1.DAL.Dto.Request;
using MoviePlatform1PL.Resourses;
using System.Security.Claims;

namespace MoviePlatform1.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICartService _cartServices;
        public CartController(ICartService cartServices, IStringLocalizer<SharedResources> stringLocalizer)
        {
            _cartServices = cartServices;
            _localizer = stringLocalizer;
        }
        [HttpPost("")]
        [Authorize]
        //بفك التوكن وبشفره ومنه بنجيب الاي دي
        public async Task<IActionResult> AddToCart(AddToCartRequest request)

        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cartServices.AddToCart(request, UserId);
            if (!result) return BadRequest();
            return Ok(new
            {
                message = _localizer["Success"].Value
            });
        }
        [HttpGet()]
        [Authorize]
        public async Task<IActionResult> GetCart()
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var items = await _cartServices.GetCart(UserId);
            return Ok(new { data = items });
        }
        [HttpDelete("{productId}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int movieId)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var removed = await _cartServices.RemoveItem(movieId, UserId);
            if (!removed) return BadRequest();
            return Ok(new { });
        }
    }
}
