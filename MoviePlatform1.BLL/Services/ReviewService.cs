
using Mapster;
using MoviePlatform1.DAL.Dto.Request;
using MoviePlatform1.DAL.Dto.Response;
using MoviePlatform1.DAL.Models;
using MoviePlatform1.DAL.Repository;

namespace MoviePlatform1.BLL.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMovieRepository _movieRepository;
        public ReviewService(IReviewRepository reviewRepository, IMovieRepository movieRepository)
        {
            _reviewRepository = reviewRepository;
            _movieRepository = movieRepository;
        }

        public async Task<ReviewResponse> AddRev(string userId, ReviewRequest request)
        {
            var existMovie = await _movieRepository.Getone(m => m.Id == request.MovieId);
            if (existMovie == null) return null;
            var exitRev = await _reviewRepository.Getone(
                r=>r.MovieId==request.MovieId&&r.UserId==userId,
                new string[]
                {
                    $"{nameof(Review.Movie)}.{nameof(Movie.Translations)}",
                    nameof(Review.User)
                   
                }
                );

            var movieRev = request.Adapt<Review>();
            movieRev.UserId = userId;

            
            await _reviewRepository.CreateAsync(movieRev);
            return movieRev.Adapt<ReviewResponse>();
        }

        public async Task<bool> DeleteReviewAsync(int movieId, string userId)
        {
            var IsRevMovie = await _reviewRepository.Getone(m => m.MovieId
                       == movieId && m.UserId == userId
                       //new string[]
                       //{

                       //nameof(Favorite.User),
                       //nameof(Favorite.Movie)
                       );
            if (IsRevMovie == null)
            {
                return false;
            }
            await _reviewRepository.DeleteAsync(IsRevMovie);
            return true;
        }        

        public async Task<List<ReviewResponse>> GetByUserIdAsync(string userId)
        {
            var revMovie = await _reviewRepository.GetAllAsync(m=>m.UserId==userId,
                 new string[]
                 {
$"{nameof(Review.Movie)}.{nameof(Movie.Translations)}"       ,
                    nameof(Review.User),

                 }
                 );
            if (revMovie == null) return null;
            return revMovie.Adapt<List<ReviewResponse>>();

        }

        public async Task<List<ReviewResponse>> GetMovieReviewsAsync(int movieId)
        {

            var revMovie = await _reviewRepository.GetAllAsync(r=>r.MovieId==movieId,
                 new string[]
                 {
$"{nameof(Review.Movie)}.{nameof(Movie.Translations)}"       ,
                    nameof(Review.User),

                 }
                 );
            if (revMovie == null) return null;
            return revMovie.Adapt<List<ReviewResponse>>();
        }

        
    }
}
