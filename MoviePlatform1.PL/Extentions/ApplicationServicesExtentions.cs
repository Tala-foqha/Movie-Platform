using Microsoft.AspNetCore.Http.HttpResults;
using MoviePlatform1.BLL.Services;
using MoviePlatform1.DAL.Repository;
using MoviePlatform1.DAL.Utils;

namespace MoviePlatform1.PL.Extentions
{
    public static class ApplicationServicesExtentions
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection Services, IConfiguration Configuration)
        {
            Services.AddScoped<IAuthenticationService, AuthenticationService>();
            Services.AddScoped<ICategoryRepository, CategoryRepository>();
            Services.AddScoped<IFileService, FileService>();
            Services.AddScoped<ICategoryService, CategoryService>();
            Services.AddScoped<IActorService, ActorService>();
            Services.AddScoped<IActorRepository, ActorRepository>();
            Services.AddScoped<IMovieRepository, MovieRepository>();
            Services.AddScoped<IMovieService, MovieService>();
            Services.AddScoped<IFavoriteRepository, FavoriteRepository>();
            Services.AddScoped<IFavoriteService, FavoriteService>();
            Services.AddScoped<IReviewRepository, ReviewRepository>();
            Services.AddScoped<IReviewService, ReviewService>();
            Services.AddScoped<INotificationService, NotificationService>();












            Services.AddScoped<ISeedData, RoleSeedData>();
            Services.AddScoped<IEmailSender, EmailSender>();


            return Services;
        }

    }
}
