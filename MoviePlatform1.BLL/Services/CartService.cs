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
    public class CartService : ICartService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly ICartRepository _cartRepository;
        public CartService(IMovieRepository movieRepository, ICartRepository cartRepository)
        {
            _movieRepository = movieRepository;
            _cartRepository = cartRepository;
        }

        public async Task<bool> AddToCart(AddToCartRequest request, string UserId)
        {
            var movie = await _movieRepository.Getone(m => m.Id == request.MovieId);
            if (movie == null)
            {
                return false;
            }
            if(!movie.IsExclusive)return false;
            var existingItem = await _cartRepository.Getone(
                m => m.MovieId == request.MovieId && m.UserId == UserId);
            if (existingItem == null)
            {
                await _cartRepository.CreateAsync(
                    new DAL.Models.Cart
                    {
                        MovieId = request.MovieId,
                        UserId = UserId,
                     
                    }
                    );
                return true;
            }
            return false;


        }

        public async Task<bool> ClearCart(string userId)
        {
            var userCart = await _cartRepository.GetAllAsync(
                m => m.UserId == userId);
            if (!userCart.Any())
            {
                return false;
            }
            return await _cartRepository.DeleteRangAsync(userCart);
        }

        public async Task<List<CartResponse>> GetCart(string userId)
        {
            var userCart = await _cartRepository.GetAllAsync(
                m => m.UserId == userId,
                new string[]
                {
                    nameof(Cart.Movie),
                    $"{nameof(Cart.Movie)}.{nameof(Movie.Translations)}"
                }
                );
            return userCart.Adapt<List<CartResponse>>();
        }

        public async Task<bool> RemoveItem(int movieId, string userId)
        {
            var items = await _cartRepository.Getone(
                c => c.MovieId == movieId & c.UserId == userId);
            if (items is null) return false;
            return await _cartRepository.DeleteAsync(items);

        }
    }
}
