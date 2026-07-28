

using Mapster;
using Microsoft.AspNetCore.Identity;
using MoviePlatform1.DAL.Dto.Request;
using MoviePlatform1.DAL.Dto.Response;
using MoviePlatform1.DAL.Models;

namespace MoviePlatform1.BLL.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthenticationService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;

        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new LoginResponse()
                {
                    Success = false,
                    Message = "Invalid Email"
                };
            }
            var res = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!res)
            {
                return new LoginResponse()
                {
                    Success = false,
                    Message = "Invalid password"
                };

            }
            return new LoginResponse()
            {
                Success = true,
                Message = "success"
            };
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            var User = request.Adapt<ApplicationUser>();
            var userManeger = await _userManager.CreateAsync(User, request.Password);

            if (!userManeger.Succeeded)
            {
                var errors = string.Join(", ", userManeger.Errors.Select(e => e.Description));

                return new RegisterResponse
                {
                    success = false,
                    Message = errors,
                    error = userManeger.Errors.Select(e => e.Description).ToList()
                };
            }
            await _userManager.AddToRoleAsync(User,"User");
            return new RegisterResponse()
            {
                success = true,
                Message = "success"
            };
        }
    }

   
       
    }
