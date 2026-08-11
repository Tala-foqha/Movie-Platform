using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MoviePlatform1.BLL.Services;
using MoviePlatform1.DAL.Dto.Request;
using MoviePlatform1.DAL.Models;
using MoviePlatform1PL.Resourses;
using System.Security.Claims;

namespace MoviePlatform1.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IOrderService _orderSevice;
        public OrderController(IOrderService orderSevice, IStringLocalizer<SharedResources> localizer)
        {
            _orderSevice = orderSevice;
            _localizer = localizer;
        }
        [HttpGet("")]
        public async Task<IActionResult> GetMyOrder()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _orderSevice.GetUserOrders(userId);
            return Ok(new { date = orders });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserOrder(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _orderSevice.GetUserOrder(userId, id);
            return Ok(new { date = order });
        }
        [HttpGet("admin")]
        [Authorize()]
        public async Task<IActionResult> GetAllOrders([FromQuery] OrderStatus status = OrderStatus.Pending)
        {
            var orders = await _orderSevice.GetAllOrders(status);
            return Ok(orders);
        }
        [HttpPatch("admon/{id}/status")]
        public async Task<IActionResult> ChangeStatus(int id, [FromBody] ChangeOrderStatus request)
        {
            var res = await _orderSevice.ChangeOrderStatus(id, request);
            if (!res) return BadRequest();
            return Ok(res);


        }



    }
}
