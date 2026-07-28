using Microsoft.EntityFrameworkCore;
using MoviePlatform1.DAL.Data;

namespace MoviePlatform1.PL.Extentions
{
    public static class DatabaseExtention
    {
        //this IServiceCollection services نوع الي رح يستخدمها والي بعده بكون اول براميتر اذا بدنا نضيف
        //Extintion method
        public static IServiceCollection AddDatabaseService(this IServiceCollection Services, IConfiguration Configuration)
        {
            Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"));
            });
            return Services;
        }

    }

}
