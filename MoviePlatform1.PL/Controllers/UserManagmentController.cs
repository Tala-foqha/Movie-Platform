
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MoviePlatform1.BLL.Services;
using MoviePlatform1.DAL.Dto.Request;
using MoviePlatform1PL.Resourses;

namespace MoviePlatform1.PL.Controllers
{
    [Route("api/UserManagment")]
    [ApiController]
    [Authorize]
    //حاليا ع الي احنا عاملينه اي حدا بقدر يشوف كل اليوزر مش بس الادمن
    public class UserManagmentController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IUserManegment _userManagement;
        public UserManagmentController(IUserManegment userManagement, IStringLocalizer<SharedResources> localizer)
        {
            _userManagement = userManagement;
            _localizer = localizer;
        }
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManagement.GetAllUser();
            return Ok(users);
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _userManagement.GetUser(id);
            return Ok(user);

        }
        [HttpPatch("{userId}/role")]
        public async Task<IActionResult> ChangeRole(string userId, [FromBody] ChangeRoleRequest request)
        {
            var result = await _userManagement.ChangeRole(userId, request.newRole);
            if (!result) return BadRequest();
            return Ok();
        }
        [HttpPatch("{userId}/toggle-bock")]
        public async Task<IActionResult> ToggleBlock(string userId)
        {
            var result = await _userManagement.ToggleBlockUser(userId);
            if (!result) return BadRequest();
            return Ok();
        }



    }
}
