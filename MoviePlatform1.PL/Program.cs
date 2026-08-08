using MoviePlatform1.BLL.Extensions;
using MoviePlatform1.DAL.Utils;
using MoviePlatform1.PL.Extentions;
using MoviePlatform1L.Extentions;
using Microsoft.AspNetCore.Mvc;

namespace MoviePlatform1.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Services

            // Localization
            builder.Services.AddLocalizationServices();

            // Controllers + show validation errors
            builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var errors = context.ModelState
                            .Where(x => x.Value.Errors.Count > 0)
                            .Select(x => new
                            {
                                Field = x.Key,
                                Errors = x.Value.Errors.Select(e => e.ErrorMessage)
                            });

                        return new BadRequestObjectResult(errors);
                    };
                });

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            // Authorization
            builder.Services.AddAuthorization();

            // Database
            builder.Services.AddDatabaseService(builder.Configuration);

            // JWT
            builder.Services.AddJWTAuthntication(builder.Configuration);

            // Identity
            builder.Services.AddIdentityServices();
            // Mapster
           
            // OpenAPI
            builder.Services.AddOpenApi();
            builder.Services.AddAplicationServices(builder.Configuration);

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddLocalizationServices();
            var app = builder.Build();
            MapsterConfig.MapsterConfigRegister(
    app.Services.GetRequiredService<IHttpContextAccessor>());


            // Request logging
            app.Use(async (context, next) =>
            {
                Console.WriteLine($"REQUEST => {context.Request.Method} {context.Request.Path}");
                await next();
            });


            if (app.Environment.IsDevelopment())
            {
                //app.UseSwagger();
                //app.UseSwaggerUI();

                app.MapOpenApi();
            }


            app.UseHttpsRedirection();

            // Images wwwroot
            app.UseStaticFiles();

            // Localization
            app.UseRequestLocalization();


            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();


            // Seed
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