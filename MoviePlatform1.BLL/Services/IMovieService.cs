using MoviePlatform1.DAL.Dto.Request;
using MoviePlatform1.DAL.Dto.Response;
using MoviePlatform1.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.BLL.Services
{
    public interface IMovieService
    {
        public Task<MovieResponse> CreateMovie(MovieRequest request);
        public Task<List<MovieResponse>> GetAllMovie();
        public Task<MovieResponse?> GetMovie(Expression<Func<Movie, bool>> filtter);
        public Task<bool> DeleteMovie(int id);
        public Task<bool> UpdateMovie(int id, MovieUpdateRequest movieUpdateRequest);


    }
}
