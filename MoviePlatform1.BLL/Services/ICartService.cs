using MoviePlatform1.DAL.Dto.Request;
using MoviePlatform1.DAL.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.BLL.Services
{
    public interface ICartService
    {
        Task<bool> AddToCart(AddToCartRequest request, string UserId);
        Task<List<CartResponse>> GetCart(string userId);
        Task<bool> RemoveItem(int movieId, string userId);
        Task<bool> ClearCart(string userId);
    }
}
