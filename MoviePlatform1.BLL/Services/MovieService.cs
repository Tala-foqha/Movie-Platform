using Azure.Core;
using Mapster;
using MoviePlatform1.BLL.Extentions;
using MoviePlatform1.DAL.Dto.Request;
using MoviePlatform1.DAL.Dto.Response;
using MoviePlatform1.DAL.Models;
using MoviePlatform1.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.BLL.Services
{
    public class MovieService : IMovieService
    {
        private readonly IFileService _fileService;
        private readonly IMovieRepository _movieRepository;
        private readonly INotificationService _notificationService;
        private readonly IWatchHistoryRepository _watchHistoryRepository;
        private readonly IUserMovieAccessRepository _userMovieAccessRepository;
        public MovieService(IFileService fileService, IMovieRepository movieRepository, INotificationService notificationService, IWatchHistoryRepository watchHistoryRepository, IUserMovieAccessRepository userMovieAccessRepository)
        {
            _fileService = fileService;
            _movieRepository = movieRepository;
            _notificationService = notificationService;
            _watchHistoryRepository = watchHistoryRepository;
            _userMovieAccessRepository = userMovieAccessRepository;
        }

        public async Task<MovieResponse> CreateMovie(MovieRequest request)
        {
            var movie = request.Adapt<Movie>();
            var existingMovie = await _movieRepository.Getone(
        m => m.Translations.Any(t =>
            t.Title == request.Translations.First().Title)
    );

            if (existingMovie != null)
            {
                return null;
            }
            movie.MovieImages ??= new List<MovieImage>();
            if (request.MainImage != null)
            {
                var imagePath = await _fileService.UploadAsync(request.MainImage);
                if (string.IsNullOrEmpty(imagePath))
                    throw new Exception("Image upload failed");
                movie.MainImage = imagePath;
                movie.price =(decimal) request.price;


            }
            if (request.MovieImages != null)
            {
                foreach (var image in request.MovieImages)
                {
                    var imagePath1 = await _fileService.UploadAsync(image);

                    if (string.IsNullOrEmpty(imagePath1))
                        continue; // أو throw حسب ما بدك

                    movie.MovieImages.Add(new MovieImage
                    {
                        imagePath = imagePath1
                    });
                }
            }
            var movie1 = await _movieRepository.CreateAsync(movie);
            var title = movie.Translations?.Where(t => t.Language == CultureInfo.CurrentCulture.Name).Select(t => t.Title).FirstOrDefault();
            await _notificationService.NotifyMovieAdded(title);
            movie1.movieUrl = request.movieUrl;


            return movie1.Adapt<MovieResponse>();
        }
        public async Task<MovieResponse?> GetMovie(Expression<Func<Movie, bool>> filtter)
        {
            var product = await _movieRepository.Getone(filtter, new string[]
            {
                nameof(Movie.Translations),
                nameof(Movie.CreateBy),
                nameof(Movie.MovieImages)
            });
            if (product == null) return null;

            return product.Adapt<MovieResponse>();

        }

        //بدل اللست صار يرجعهم كبجنيشن ريسبونس
        //ببجيب الافلام بعمل الفلتر بعدين الباجنيشن
        public async Task<PaginationResponse<MovieResponse>> GetAllMovie(MovieFiltterRequest request)
        {
            var query =  _movieRepository.GetQureable(
                c => c.Status == EntityStatus.Active,
                new string[]
                {
                   nameof(Movie.Translations),
                   nameof(Movie.CreateBy),
                   nameof(Movie.MovieImages),
                   nameof(Movie.MovieCategories)


                }
                );
            if (request.Search != null)
            {
                query = query.Where(m => m.Translations.Any(t => t.Title.Contains(request.Search)));
            }
            if (request.CategoryId.HasValue)
            {
                query = query.Where(m => m.MovieCategories.Any(c => c.CategoryId == request.CategoryId));
            }
            if (request.IsExclusive.HasValue)
            {
                query = query.Where(m => m.IsExclusive == request.IsExclusive); 

            }
            if (request.IsExclusive == true)

            {

                if (request.MinPrice.HasValue)

                {

                    query = query.Where(m =>

                        m.price >= request.MinPrice.Value);

                }

                if (request.MaxPrice.HasValue)

                {

                    query = query.Where(m =>

                        m.price <= request.MaxPrice.Value);

                }
            }
                var paginated = await query.ToPaginationasync(request.Page, request.Limit);

            return new PaginationResponse<MovieResponse>
            {
                Data = paginated.Data.Adapt<List<MovieResponse>>(),
                TotalCount = paginated.TotalCount,
                Page = paginated.Page,
                                Limit = paginated.Limit



            };
        }

        public async Task<bool> UpdateMovie(int id, MovieUpdateRequest movieUpdateRequest)
        {
            var movie = await _movieRepository.Getone(m => m.Id == id,
                new string[]
                {
                    nameof(Movie.Translations),
                    nameof(Movie.CreateBy),
                    nameof(Movie.MovieImages)

                }
                );
            if (movie == null) return false;
            if (movieUpdateRequest.AgeRating != null)
            {
                movie.AgeRating = movieUpdateRequest.AgeRating;
            }
            var oldImage = movie.MainImage;
            if (movieUpdateRequest.Translations != null)
            {
                foreach (var translationRequest in movieUpdateRequest.Translations)
                {
                    var exisiting = movie.Translations.FirstOrDefault(p => p.Language == translationRequest.Language);
                    if (exisiting != null)
                    {
                        if (translationRequest.Title != null)
                        {
                            exisiting.Title = translationRequest.Title;
                        }

                        if (translationRequest.Description != null)
                        {
                            exisiting.Description = translationRequest.Description;
                        }
                    }
                }
            }
            if (movieUpdateRequest.MainImage != null)
            {
                _fileService.Delete(oldImage);
                movie.MainImage = await _fileService.UploadAsync(movieUpdateRequest.MainImage);
            }
            else
            {
                movie.MainImage = oldImage;
            }
            if (movieUpdateRequest.SubImages != null)
            {
                foreach (var image in movie.MovieImages)
                {
                    _fileService.Delete(image.imagePath);

                }
                movie.MovieImages.Clear();

                foreach (var image in movieUpdateRequest.SubImages)
                {
                    var imagePath = await _fileService.UploadAsync(image);

                    movie.MovieImages.Add(new MovieImage
                    {
                        imagePath = imagePath
                    });
                }
            }
            var title = movie.Translations?.Where(t => t.Language == CultureInfo.CurrentCulture.Name).Select(t => t.Title).FirstOrDefault();

            await _notificationService.NotifyMovieUpdated(title);
            return await _movieRepository.UpdateAsync(movie);
        }

        public async Task<bool> DeleteMovie(int id)
        {
            var movie = await _movieRepository.Getone(m => m.Id == id,
                new string[]
                {
                    nameof(Movie.Translations),
                    nameof(Movie.MovieImages)
                }
                );
            if (movie == null)
            {
                return false;

            }
            _fileService.Delete(movie.MainImage);
            foreach (var image in movie.MovieImages)
            {
                _fileService.Delete(image.imagePath);
            }
            return await _movieRepository.DeleteAsync(movie);

        }

        public async Task<WatchMovieResponse?> WatchMovie(
     int movieId,
     string userId)
        {
            var movie = await _movieRepository.Getone(
                m => m.Id == movieId);

            if (movie == null)
                return null;

            // Free movie
            if (!movie.IsExclusive)
            {
                await _watchHistoryRepository.CreateAsync(
                    new WatchHistory
                    {
                        UserId = userId,
                        MovieId = movieId,
                        WatchedAt = DateTime.Now
                    });

                return new WatchMovieResponse
                {
                    CanWatch = true,
                    Message = "This movie is free. You can watch it.",
                    MovieUrl = movie.movieUrl
                };
            }

            // Exclusive movie
            var access = await _userMovieAccessRepository.Getone(
                x => x.UserId == userId &&
                     x.MovieId == movieId);

            if (access == null || !access.HasAccess)
            {
                return new WatchMovieResponse
                {
                    CanWatch = false,
                    Message = "This movie is exclusive. Please purchase it.",
                    MovieUrl = null
                };
            }

            // Has access
            await _watchHistoryRepository.CreateAsync(
                new WatchHistory
                {
                    UserId = userId,
                    MovieId = movieId,
                    WatchedAt = DateTime.Now
                });

            return new WatchMovieResponse
            {
                CanWatch = true,
                Message = "You have access to this movie.",
                MovieUrl = movie.movieUrl
            };
        }
    }
}
