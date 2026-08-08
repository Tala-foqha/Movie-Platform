using Mapster;
using MoviePlatform1.DAL.Dto.Request;
using MoviePlatform1.DAL.Dto.Response;
using MoviePlatform1.DAL.Models;
using MoviePlatform1.DAL.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.AspNetCore.Server.IIS;
using Microsoft.AspNetCore.Http;

namespace MoviePlatform1.BLL.Extensions
{
    public static class MapsterConfig
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static void MapsterConfigRegister(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            TypeAdapterConfig<Category, CategoryResponse>.NewConfig()
   .Map(dest => dest.Category_Id, src => src.Id)
   //.Map(dest => dest.UserCreated, src => src.CreateBy)
   .Map(dest => dest.MainImage, src => BuildImageUrl(src.ImageUrl))
   .Map(dest => dest.Name,
        src => src.translations != null
            ? src.translations
                .Where(t => t.Language == CultureInfo.CurrentCulture.Name)
                .Select(t => t.Name)
                .FirstOrDefault()
            : null)
   ;
            TypeAdapterConfig<Favorite, FavoriteResponse>.NewConfig()
   .Map(dest => dest.FavId, src => src.FavoriteId)
   .Map(dest=>dest.MovieId,src=>src.Movie.Id)
   .Map(dest => dest.UserName, src => src.User.FullName)
    .Map(dest => dest.MainImage,
        src => BuildImageUrl(src.Movie.MainImage))
   .Map(dest => dest.MovieTitle,
        src => src.Movie != null && src.Movie.Translations != null
            ? src.Movie.Translations
                .Where(t => t.Language == CultureInfo.CurrentCulture.Name)
                .Select(t => t.Title)
                .FirstOrDefault()
            : null);
            
            TypeAdapterConfig<Actor, ActorResponse>.NewConfig()
    .Map(dest => dest.ActorId, src => src.Id)
    .Map(dest => dest.MainImage, src => BuildImageUrl(src.ImageUrl))
    .Map(dest => dest.FirstName,
        src => src.ActorTranslations
            .Where(t => t.Language == CultureInfo.CurrentCulture.Name)
            .Select(t => t.FirstName)
            .FirstOrDefault())
    .Map(dest => dest.LastName,
        src => src.ActorTranslations
            .Where(t => t.Language == CultureInfo.CurrentCulture.Name)
            .Select(t => t.LastName)
            .FirstOrDefault());
            ;
            
            TypeAdapterConfig<MovieRequest, Movie>.NewConfig()
                .Ignore(dest => dest.MovieImages);


            TypeAdapterConfig<Movie, MovieResponse>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                //.Map(dest=>dest.UserCreated,src=>src.CreateBy.UserName)
                .Map(dest => dest.MainImage, src => BuildImageUrl(src.MainImage))
                .Map(dest => dest.Images,
                     src => src.MovieImages.Select(i => BuildImageUrl(i.imagePath)))
                .Map(dest => dest.Name,
                     src => src.Translations != null
                         ? src.Translations
                             .Where(t => t.Language == CultureInfo.CurrentCulture.Name)
                             .Select(t => t.Title)
                             .FirstOrDefault()
                         : null);
        }


        private static string BuildImageUrl(string image)
        {
            if (string.IsNullOrEmpty(image)) return null;
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null)
                return $"/image/{image}";
            return $"{request.Scheme}://{request.Host}/images/{image}";
         }
    }
}
