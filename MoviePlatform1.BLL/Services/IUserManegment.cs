using KASHOP.DAL.Dto.Response;
using MoviePlatform1.DAL.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.BLL.Services
{
    public interface IUserManegment
    {
        Task<bool> ChangeRole(string userId, string Role);

        Task<bool> DeleteUser(string userId);

        Task<List<UserListResponse>> GetAllUser();

        Task<UserDetailsResponse> GetUser(string userId);

        Task<bool> ToggleBlockUser(string userId);
    }
}
