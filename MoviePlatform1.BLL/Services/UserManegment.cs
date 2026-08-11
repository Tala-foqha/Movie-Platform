using KASHOP.DAL.Dto.Response;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MoviePlatform1.DAL.Dto.Response;
using MoviePlatform1.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.BLL.Services
{
    public class UserManegment : IUserManegment
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserManegment(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> ChangeRole(string userId, string Role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roleExists = await _roleManager.RoleExistsAsync(Role);
            if (!roleExists) return false;
            var currentRole = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRole);
            var result = await _userManager.AddToRoleAsync(user, Role);
            return result.Succeeded;
        }

        public Task<bool> DeleteUser(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<UserListResponse>> GetAllUser()
        {
            var users = await _userManager.Users.ToListAsync();

            var result = new List<UserListResponse>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                result.Add(new UserListResponse
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    IsBlocked = user.LockoutEnd.HasValue,
                    role = roles.FirstOrDefault()
                });
            }

            return result;
        }


        public async Task<UserDetailsResponse> GetUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            var result = user.Adapt<UserDetailsResponse>();
            result.role = roles.FirstOrDefault();//هاي ليست بس الي احنا عنا بس ستريم الي هي صلاحية وحدة فبنعمل هيك وبنجيب اول صلاحية وخلص

            return result;
        }

        public async Task<bool> ToggleBlockUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return false;

            bool isBlocked = user.LockoutEnd.HasValue &&
                             user.LockoutEnd.Value > DateTimeOffset.UtcNow;

            if (isBlocked)
            {
                // Unblock
                await _userManager.SetLockoutEndDateAsync(user, null);
            }
            else
            {
                // Block for 5 days
                await _userManager.SetLockoutEnabledAsync(user, true);
                await _userManager.SetLockoutEndDateAsync(
                    user,
                    DateTimeOffset.UtcNow.AddDays(5)
                );
            }

            return true;
        }
    }
}
