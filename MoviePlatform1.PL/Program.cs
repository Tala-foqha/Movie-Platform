
using MoviePlatform1.DAL.Utils;
using MoviePlatform1.PL.Extentions;

namespace MoviePlatform1.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAplicationServices(builder.Configuration);



            builder.Services.AddControllers();
            builder.Services.AddAuthorization();



            builder.Services.AddDatabaseService(builder.Configuration);
            builder.Services.AddIdentityServices();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            //وظيفته يشغل اليد داتا اول ما نشغل التطبيق
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var seeders = services.GetServices<ISeedData>();

                foreach (var seeder in seeders)
                {
                    try
                    {
                       await seeder.DataSeed();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Seeding Error: {ex.Message}");
                    }
                }
            }

            app.Run();
        }
    }
}
