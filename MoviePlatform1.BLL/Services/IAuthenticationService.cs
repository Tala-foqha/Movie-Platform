using MoviePlatform1.DAL.Dto.Request;
using MoviePlatform1.DAL.Dto.Response;


namespace MoviePlatform1.BLL.Services
{
    public interface IAuthenticationService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);
        Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<bool> confirmEmailAsync(String token, String userId);
        Task<ForgotPasswordResponse> RequestPasswordReset(ForgotPasswordRequest request);
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request);
        Task<LoginResponse> RefreshTokenAsync();

    }
}
