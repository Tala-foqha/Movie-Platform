using MoviePlatform1.DAL.Dto.Request;
using MoviePlatform1.DAL.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.BLL.Services
{
    public interface IFavoriteService
    {
        public Task<FavoriteResponse> ToggleFavoriteAsync(FavoriteRequest request,string userId);
        public Task<bool> DeleteAsync(string userId, int movieId);
        public Task<List<FavoriteResponse>> GetByUserId(string userId);
        public Task<bool>IsFavoriteAsync(string userId, int movieId);
    }
}
