

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
            var refreshToken=await GenerateRefreshToken(user);
            SertRefreshTokenCookies(refreshToken);


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
            var roleResult = await _userManager.AddToRoleAsync(User,"User");
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ",
                    roleResult.Errors.Select(e => e.Description));

                return new RegisterResponse
                {
                    success = false,
                    Message = errors,
                    error = roleResult.Errors
                        .Select(e => e.Description)
                        .ToList()
                };
            }
            //للتاكد  عشان اعرف اليوزر وصل بشكل صحيح ولا لا
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(User);
            // بحول التوكن لصيغة امنة لتفادي اي خطأ هاد النص بنعمل مرة وحدة م حد بوخذه
            token = Uri.EscapeDataString(token);
            var emailUrl = $"{_httpContext.HttpContext.Request.Scheme}://{_httpContext.HttpContext.Request.Host}/api/Account/confirmemail?token={token}&userId={User.Id}";
            await _emailSender.SendEmail(
                email: User.Email,
                subject: "welcome",
                message:
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
        expires: DateTime.Now.AddMonths(5),
        signingCredentials: credentials
        );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async  Task<ForgotPasswordResponse> RequestPasswordReset(ForgotPasswordRequest request)
        {
           var user=await _userManager.FindByEmailAsync(request.Email);//return applicationuser nullble
            if (user == null)
                return new ForgotPasswordResponse()
                {
                    Message = "Email not found",
                    Success = false
                };
            //create code
            var random=new Random();
            //بدي من هاد الراندوم اربع حروف والرينج
          var code=  random.Next(1000,10000).ToString();
            user.CodeRequestPassword = code;
            user.PasswordResetCodeExpiry = DateTime.UtcNow.AddMinutes(15);
            //تحديث المعلومات
            await _userManager.UpdateAsync(user);
            await _emailSender.SendEmail(
                email:user.Email,
                subject:"Reset password",
                message:$"<p>Code Is {code}</p>"
                );
            return new ForgotPasswordResponse()
            {
                Message = "Code Sent to your email",
                Success = true
            };
        }

        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return new ResetPasswordResponse()
                {
                    Message = "Email is not found",
                    Success = false
                };
            if (user.CodeRequestPassword != request.Code)
            {
                return new ResetPasswordResponse()
                {
                    Message = "Invalid code",
                    Success = false
                };
            }
            if (user.PasswordResetCodeExpiry<DateTime.UtcNow)
                return new ResetPasswordResponse()
                {
                    Message = "Code Ecpired",
                    Success = false
                };
            //نفحص اذا الباسورد الجديد نفس القديم عن طريق ميثود
            var isSamePassword = await _userManager.CheckPasswordAsync(user, request.NewPassword);
            if (isSamePassword)
            {
                return new ResetPasswordResponse()
                {
                    Message = "password must diffrent from old password",
                    Success = false
                };
            };
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user,token, request.NewPassword);
            if (!result.Succeeded)
            {
                return new ResetPasswordResponse()
                {
                    Message = string.Join(", ", result.Errors.Select(e => e.Description)),
                    Success = false
                };
            }
            //من جواها بتعمل ابديت للداتا بيس

            await _emailSender.SendEmail(user.Email, "change Password", "<p>Your password is changed</p>");

            return new ResetPasswordResponse()
            {
                Message = "password reset successfully",
                Success = true
            };
        }
        private async Task<String> GenerateRefreshToken(ApplicationUser user)
        {
            var refreshToken=Guid.NewGuid().ToString();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddMinutes(15);
            await _userManager.UpdateAsync(user);
            return refreshToken;
        }
        //each func do one task only
        //fun that set in cookies
        private void SertRefreshTokenCookies(string refreshToken)
        {
            _httpContext.HttpContext.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true, //important
                Secure = false, //مستحيل اقبل اي ريكوست جاي من http only https ,true for production
                SameSite = SameSiteMode.None,// بنخليها ستريكت لما نرفع المشروع
                //لما تقبل اي ريكويست اقبلو من موقعني انا اذا كانت ستركت
                Expires = DateTime.UtcNow.AddMinutes(15)
            });
        }

        public async Task<LoginResponse> RefreshTokenAsync()
        {
            var refreshToken = _httpContext.HttpContext.Request.Cookies["refreshToken"];//جبلي اياها from http only

            if (refreshToken == null)
            {
                return new LoginResponse()
                {
                    Success = false,
                    Message = "no refresh token"
                };
            }
            //اجيب معلومات الوزر الي اله هاد الرفريش  توكن
            var user = _userManager.Users.FirstOrDefault(u => u.RefreshToken == refreshToken);
            if(user.RefreshTokenExpiry<DateTime.UtcNow)
            {
                return new LoginResponse()
                {
                    Success = false,
                    Message = "refresh token expried"
                };
            }

            var newRefreshToken = await GenerateRefreshToken(user);
            SertRefreshTokenCookies(newRefreshToken);
            return new LoginResponse()
            {
                Message="success",
                Success = true,

                AccessToken = await GenerateAccessToken(user),
            };

        }
                }

    }


   