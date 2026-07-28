

using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MoviePlatform1.DAL.Dto.Request;
using MoviePlatform1.DAL.Dto.Response;
using MoviePlatform1.DAL.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MoviePlatform1.BLL.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IConfiguration _configuration;
        public AuthenticationService(UserManager<ApplicationUser> userManager, IEmailSender emailSender, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _emailSender = emailSender;
            _configuration = configuration;
            _userManager = userManager;
            _httpContext = httpContextAccessor;


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
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return new LoginResponse()
                {
                    Success = false,
                    Message = "Email is not confirmed"
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
                Message = "success",
                AccessToken = await GenerateAccessToken(user)
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
            await _userManager.AddToRoleAsync(User, "User");
            //للتاكد  عشان اعرف اليوزر وصل بشكل صحيح ولا لا
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(User);
            // بحول التوكن لصيغة امنة لتفادي اي خطأ هاد النص بنعمل مرة وحدة م حد بوخذه
            token = Uri.EscapeDataString(token);
            var emailUrl = $"{_httpContext.HttpContext.Request.Scheme}://{_httpContext.HttpContext.Request.Host}/api/Account/confirmemail?token={token}&userId={User.Id}";
            await _emailSender.SendEmail(
                email: User.Email,
                subject: "welcome",
                $"<h1>Welcome {request.UserName}</h1>" +
                $"<p>Please confirm your email:</p>" +
                $"<a href=\"{emailUrl}\">Confirm Email</a>"

                );
            return new RegisterResponse()
            {
                success = true,
                Message = "success"
            };
        }
        public async Task<bool> confirmEmailAsync(String token, String userId)
        {
            //تتأكد اليوزر موجود ولا لا
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return false;
            //بتغير الايميل كونفيرمد من فولس لترو يعني بتعمل التأكيد 
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return false;
            }
            return true;
        }
        private async Task<String> GenerateAccessToken(ApplicationUser user)
        {
            //أكثر من داتا بدي اخزنها
            //اول مجال ممكن نحط سترينج احنا ونكتبه ولك في مسميات جاهزة
            var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //هيك بنشئ التوكن وبنحط جواها كل القيم الي عملناهم و3 اجزاء الي لازم يكزنو فيها 
            var token = new JwtSecurityToken(
        issuer: _configuration["Jwt:Issuer"],
        audience: _configuration["Jwt:Audience"],
        claims: userClaims,
        expires: DateTime.Now.AddDays(5),
        signingCredentials: credentials
        );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

   