using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviePlatform1.BLL.Services;
using MoviePlatform1.DAL.Dto.Request;
using System.Security.Claims;

namespace MoviePlatform1.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritsController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;
        public FavoritsController (IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ToggleFavorite(FavoriteRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _favoriteService.ToggleFavoriteAsync(request, userId);

            if (result == null)
            {
                return BadRequest("Movie not found or removed from favorites");
            }

            return Ok(result);
        }
            // Delete Favorite
     
        [Authorize]
        [HttpDelete("{movieId}")]
        public async Task<IActionResult> DeleteFavorite(int movieId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _favoriteService.DeleteAsync(userId, movieId);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Movie not found in favorites"
                });
            }

            return Ok(new
            {
                message = "Movie removed from favorites"
            });
        }
    


            // Get Current User Favorites
            [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMyFavorites()
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                    return Unauthorized();


                var result = await _favoriteService.GetByUserId(userId);


                if (result == null)
                {
                    return Ok(new List<object>());
                }


                return Ok(new
                {
                    date=result
                });
            }



            // Check if movie is favorite
            [HttpGet("  /{movieId}")]
        [Authorize]
            public async Task<IActionResult> IsFavorite(int movieId)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                    return Unauthorized();


                var result = await _favoriteService.IsFavoriteAsync(userId, movieId);


                return Ok(new
                {
                    movieId = movieId,
                    isFavorite = result
                });
            }
        }
    }



