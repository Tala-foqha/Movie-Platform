using Microsoft.AspNetCore.Http.HttpResults;
using MoviePlatform1.BLL.Services;
using MoviePlatform1.DAL.Repository;
using MoviePlatform1.DAL.Utils;
using Stripe;
using FileService = MoviePlatform1.BLL.Services.FileService;
using ReviewService = MoviePlatform1.BLL.Services.ReviewService;


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
            Services.AddScoped<IWatchHistoryRepository, WatchHistoryRepository>();
            Services.AddScoped<IUserMovieAccessRepository, UserMovieAccessRepository>();
            Services.AddScoped<ICartRepository, CartRepository>();
            Services.AddScoped<ICartService, CartService>();
            Services.AddScoped<ISeedData, RoleSeedData>();
            Services.AddScoped<IUserManegment, UserManegment>();





            Services.AddScoped<IOrderRepository, OrderRepository>();

            Services.AddScoped<IOrderService, OrderService>();
            Services.AddScoped<ICheckoutService, BLL.Services.CheckoutService>();









            Services.Configure<StripeSetting>(Configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey = Configuration["Stripe:SecretKey"];






















            Services.AddScoped<IEmailSender, EmailSender>();


            return Services;
        }

    }
}
