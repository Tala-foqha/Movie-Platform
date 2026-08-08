using Microsoft.AspNetCore.Identity;
using MoviePlatform1.DAL.Data;
using MoviePlatform1.DAL.Models;

namespace MoviePlatform1.PL.Extentions
{
    public static class IdentityExtention
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection Services)
        {
            Services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.User.RequireUniqueEmail = true;

                // Password validation
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 10;

                // Lockout
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            return Services;
        }
    }
}