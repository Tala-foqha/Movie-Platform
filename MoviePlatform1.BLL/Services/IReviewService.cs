using MoviePlatform1.DAL.Dto.Request;
using MoviePlatform1.DAL.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.BLL.Services
{
    public interface IReviewService
    {
        Task<ReviewResponse> AddRev(string userId, ReviewRequest request);
        //all review to a specfic film
        Task<List<ReviewResponse>>GetMovieReviewsAsync(int movieId);

        Task<List<ReviewResponse>>GetByUserIdAsync(string userId);

        //Task UpdateReviewAsync(int id, ReviewRequest request);

        Task<bool>DeleteReviewAsync(int movieId, string userId);

    }
}
