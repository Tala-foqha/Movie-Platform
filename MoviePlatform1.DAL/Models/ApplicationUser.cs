using Microsoft.AspNetCore.Identity;

namespace MoviePlatform1.DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public String FullName { get; set; }
        public String? CodeRequestPassword { get; set; }
        public DateTime? PasswordResetCodeExpiry { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }

    }
}
