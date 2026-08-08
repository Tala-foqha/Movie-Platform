using Mapster;
using MoviePlatform1.DAL.Dto.Request;
using MoviePlatform1.DAL.Dto.Response;
using MoviePlatform1.DAL.Models;
using MoviePlatform1.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.BLL.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IMovieRepository _movieRepository;
        public FavoriteService(IFavoriteRepository favoriteRepository,IMovieRepository movieRepository)

        {
            _movieRepository = movieRepository;
            _favoriteRepository = favoriteRepository;

        }

        public async Task<FavoriteResponse?> ToggleFavoriteAsync(FavoriteRequest request, string userId)
        {
            // 1- Check if movie exists
            var movie = await _movieRepository.Getone(
                m => m.Id == request.movieId,
                new string[]
                {
            nameof(Movie.Translations)
                }
            );

            if (movie == null)
                return null;


            // 2- Check if movie already exists in user's favorites
            var existInFavorite = await _favoriteRepository.Getone(
                f => f.movieId == request.movieId && f.UserId == userId,
                new string[]
                {
            nameof(Favorite.Movie),
            nameof(Favorite.User)
                }
            );


            // 3- If exists, remove it
            if (existInFavorite != null)
            {
                await _favoriteRepository.DeleteAsync(existInFavorite);

                return null; // removed from favorites
            }


            // 4- If not exists, add it
            var favMovie = new Favorite
            {
                movieId = request.movieId,
                UserId = userId
            };

            var addFavMovie = await _favoriteRepository.CreateAsync(favMovie);


            // Reload with relations for Mapster
            var favorite = await _favoriteRepository.Getone(
                f => f.FavoriteId == addFavMovie.FavoriteId,
                new string[]
                {
            nameof(Favorite.Movie),
            nameof(Favorite.User)
                }
            );

            return favorite.Adapt<FavoriteResponse>();
        }

        public async Task<bool> DeleteAsync(string userId, int movieId)
        {
            var IsFavMovie = await _favoriteRepository.Getone(m => m.movieId
            == movieId && m.UserId == userId
            //new string[]
            //{

            //nameof(Favorite.User),
            //nameof(Favorite.Movie)
            );
            if (IsFavMovie == null)
            {
                return false;
            }
            await _favoriteRepository.DeleteAsync(IsFavMovie);
            return true;
        }

        public async Task<List<FavoriteResponse>> GetByUserId(string userId)
        {
            var favMovie = await _favoriteRepository.GetAllAsync(u => u.UserId == userId,
                new string[]
                {
$"{nameof(Favorite.Movie)}.{nameof(Movie.Translations)}"       ,
                    nameof(Favorite.User),
                  
                }
                );
            if (favMovie == null) return null;
            return  favMovie.Adapt<List<FavoriteResponse>>();
        }

        public async Task<bool> IsFavoriteAsync(string userId, int movieId)
        {
            var favorite = await _favoriteRepository.Getone(
                f => f.UserId == userId && f.movieId == movieId
            );

            return favorite != null;
        }
    }
}
