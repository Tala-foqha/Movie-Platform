using Microsoft.AspNetCore.Identity;
using MoviePlatform1.DAL.Data;
using MoviePlatform1.DAL.Models;

namespace MoviePlatform1.PL.Extentions
{
    public static class IdentityExtention
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection Services)
        {
            Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                //validation for register
                options.Password.RequireDigit = true;//0-9
                options.Password.RequireLowercase = true;//a-z
                options.Password.RequireUppercase = true;//A-Z

                options.Password.RequireNonAlphanumeric = true;// romoz !@#$%
                options.Password.RequiredLength = 10;
                options.Lockout.MaxFailedAccessAttempts = 5;// اايوزر معه خمس محاولات يسجل الباسورد غلط
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                //بنعمل لليوزر بلوك لمدة عشر دقائق

            })
           .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
                   return Services;
        }
    }
}

