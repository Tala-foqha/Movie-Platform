using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MoviePlatform1.PL.Extentions
{
    public static class AuthenticationExtentions
    {
        public static IServiceCollection AddJWTAuthntication(this IServiceCollection Services,IConfiguration Configuration)
        {
          Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(options =>
           {
               var key =Configuration["Jwt:SecretKey"];

               if (string.IsNullOrEmpty(key))
                   throw new Exception("JWT SecretKey is missing!");

               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = true,
                   ClockSkew= TimeSpan.Zero,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = Configuration["Jwt:Issuer"],
                   ValidAudience = Configuration["Jwt:Audience"],
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
               };
           });
            return Services;
        }
    }
}
