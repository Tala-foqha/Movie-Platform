using Mapster;
using MoviePlatform1.DAL.Dto.Request;
using MoviePlatform1.DAL.Dto.Response;
using MoviePlatform1.DAL.Models;
using MoviePlatform1.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFileService _fileService;
        public CategoryService(ICategoryRepository categoryRepository,IFileService fileService)
        {
            _categoryRepository = categoryRepository;
            _fileService = fileService;
        }
        public async Task<CategoryResponse> CreateCategory(CategoryRequest request, string lang = "en")
        {
           

            var category = request.Adapt<Category>();
            var categoryExists = await _categoryRepository.Getone(
        m => m.translations.Any(t =>
            t.Name == request.translations.First().Name)
    );

            if (categoryExists != null)
            {
                return null;
            }


            if (request.MainImage != null)
            {
                var imagePath = await _fileService.UploadAsync(request.MainImage);

                if (string.IsNullOrEmpty(imagePath))
                {
                    throw new Exception("Image upload failed");
                }

                category.ImageUrl = imagePath;
            }

            await _categoryRepository.CreateAsync(category);

            return category
      .Adapt<CategoryResponse>();
        }
        public async Task<List<CategoryResponse>> GetAllCategories(string lang = "en")
        {
            var categories = await _categoryRepository.GetAllAsync(
                c => c.Status == EntityStatus.Active,
                new string[]
                {
                   nameof(Category.translations),
                   nameof(Category.CreateBy)
                }
                );
            var response = categories.BuildAdapter().AddParameters("lang", lang).AdaptToType<List<CategoryResponse>>();
            return response;
        }

        public async Task<CategoryResponse?> GetCategory(Expression<Func<Category, bool>> filter)
        {
            var category = await _categoryRepository.Getone(filter, new string[]
            {
                nameof(Category.translations),
                  nameof(Category.CreateBy)
            });
            return category.Adapt<CategoryResponse>();
        }
        public async Task<bool> DeleteCategory(int id)
        {
            var category = await _categoryRepository.Getone(c => c.Id == id);
            if (category == null)
            {
                return false;

            }
             _fileService.Delete(category.ImageUrl);
            return await _categoryRepository.DeleteAsync(category);


        }
        public async Task<bool> UpdateCategory(int id, CategoryUpdateRequest request)
        {
            var category = await _categoryRepository.Getone(c => c.Id == id,
                new string[]
                {
                    nameof(Category.translations),
                }
                );
            if (category == null)
            {
                return false;
            }
            var oldImage = category.ImageUrl;

            var category1 = request.Adapt(category);
            if (request.translations != null)
            {
                foreach (var translationRequest in request.translations)
                {
                    var existing = category1.translations.FirstOrDefault(c => c.Language == translationRequest.Language);
                    if (existing != null)
                    {
                        if (translationRequest.Name != null)
                        {
                            existing.Name = translationRequest.Name;
                        }

                    }
                }
            }
            if (request.MainImage != null)
            {
                _fileService.Delete(oldImage);
                category.ImageUrl = await _fileService.UploadAsync(request.MainImage);
            }

                return await _categoryRepository.UpdateAsync(category);
            }
        }
    }

