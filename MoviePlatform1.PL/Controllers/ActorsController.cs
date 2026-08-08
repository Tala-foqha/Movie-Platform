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
    public class ActorsController : ControllerBase
    {
        private readonly IActorService _actorService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public ActorsController(IActorService actorService, IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            _actorService = actorService;
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] ActorRequest request)
        {
            
            //var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var lang = Request.Headers["Accept-Language"].ToString();
            var category = await _actorService.CreateActorAsync(request);
            return Ok(new
            {
                data = category
            });

           
        }
        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var lang = Request.Headers["Accept-Language"].ToString();
            var actors = await _actorService.GetAllActors();
            return Ok(
                new
                {
                    data = actors,
                    message = "Success"
                }
                );
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            return Ok(await _actorService.GetActor(c => c.Id == Id));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var delete = await _actorService.DeleteActor(id);
            if (!delete)
            {
                return NotFound(new
                {
                    message = _localizer["NotFound"].Value
                });
            }
            return NotFound(new
            {
                message = _localizer["Success"].Value
            });
        }
    }
}
