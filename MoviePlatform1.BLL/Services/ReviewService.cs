
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
        private readonly IWatchHistoryRepository _watchHistoryRepository;
        public ReviewService(IReviewRepository reviewRepository, IMovieRepository movieRepository, IWatchHistoryRepository watchHistoryRepository   )
        {
            _reviewRepository = reviewRepository;
            _movieRepository = movieRepository;
            _watchHistoryRepository = watchHistoryRepository;
        }

        public async Task<ReviewResponse> AddRev(
     string userId,
     ReviewRequest request)
        {
            // Check movie exists
            var existMovie = await _movieRepository.Getone(
                m => m.Id == request.MovieId
            );

            if (existMovie == null)
                return null;

            // Check if user watched the movie
            var watched = await _watchHistoryRepository.Getone(
                w => w.MovieId == request.MovieId &&
                     w.UserId == userId
            );

            if (watched == null)
                return null;

            // Check if user already reviewed the movie
            var existRev = await _reviewRepository.Getone(
                r => r.MovieId == request.MovieId &&
                     r.UserId == userId,
                new string[]
                {
            nameof(Review.User),
            $"{nameof(Review.Movie)}.{nameof(Movie.Translations)}"
                }
            );

            if (existRev != null)
                return null;

            // Create review
            var movieRev = request.Adapt<Review>();
            movieRev.UserId = userId;

            await _reviewRepository.CreateAsync(movieRev);

            // Get review again with User and Movie
            var result = await _reviewRepository.Getone(
                r => r.Id == movieRev.Id,
                new string[]
                {
            nameof(Review.User),
            $"{nameof(Review.Movie)}.{nameof(Movie.Translations)}"
                }
            );

            return result.Adapt<ReviewResponse>();
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
