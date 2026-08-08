using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviePlatform1.BLL.Services;
using MoviePlatform1.DAL.Dto.Request;
using System.Security.Claims;


namespace MoviePlatform1.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }


        // Add Review
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddReview(ReviewRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _reviewService.AddRev(userId, request);

            if (result == null)
                return BadRequest("Movie not found or already reviewed");

            return Ok(result);
        }


        // Get all reviews for specific movie
        [AllowAnonymous]
        [HttpGet("movie/{movieId}")]
        public async Task<IActionResult> GetMovieReviews(int movieId)
        {
            var result = await _reviewService.GetMovieReviewsAsync(movieId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }


        // Get reviews of current user
        [Authorize]
        [HttpGet("my-reviews")]
        public async Task<IActionResult> GetMyReviews()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _reviewService.GetByUserIdAsync(userId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }


        // Delete review by movieId (current user)
        [Authorize]
        [HttpDelete("{movieId}")]
        public async Task<IActionResult> DeleteReview(int movieId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _reviewService.DeleteReviewAsync(movieId, userId);

            if (!result)
                return NotFound("Review not found");

            return NoContent();
        }
    }
}