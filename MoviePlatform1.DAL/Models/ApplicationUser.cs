using Microsoft.AspNetCore.Identity;

namespace MoviePlatform1.DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public String FullName { get; set; }
        public String? CodeRequestPassword { get; set; }//عشان هاد الكود رح نقارنه فلازم يتخزن بجدول اليوزر
        public DateTime? PasswordResetCodeExpiry { get; set; }
        //for refresh token
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
     

    }
}
