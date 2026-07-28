using MoviePlatform1.BLL.Services;
using MoviePlatform1.DAL.Utils;

namespace MoviePlatform1.PL.Extentions
{
    public static class ApplicationServicesExtentions
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection Services, IConfiguration Configuration)
        {
            Services.AddScoped<IAuthenticationService, AuthenticationService>();
            Services.AddScoped<ISeedData, RoleSeedData>();
            Services.AddScoped<IEmailSender, EmailSender>();


            return Services;
        }

    }
}
