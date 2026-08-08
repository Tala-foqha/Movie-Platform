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
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public MovieController(IMovieService movieService, IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            _movieService = movieService;
        }
        [HttpPost]


        [Authorize]
        public async Task<IActionResult> Create([FromForm] MovieRequest request)
        {
            
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var lang = Request.Headers["Accept-Language"].ToString();
            var category = await _movieService.CreateMovie(request);
            return Ok(new
            {
                data = category
            })
            ;
            //return Ok("وصل");
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var lang = Request.Headers["Accept-Language"].ToString();
            var categories = await _movieService.GetAllMovie();
            return Ok(
                new
                {
                    data = categories
                    ,
                    message = _localizer["Success"].Value
                });
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            return Ok(await _movieService.GetMovie(c => c.Id == Id));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id,int movieId)
        {
            var delete = await _movieService.DeleteMovie(id);
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
        [HttpPatch("{Id}")]
        public async Task<IActionResult> Update(int id, [FromForm] MovieUpdateRequest request)
        {
            var response = await _movieService.UpdateMovie(id, request);
            if (!response) return BadRequest();
            return Ok();
        }
    }


}
