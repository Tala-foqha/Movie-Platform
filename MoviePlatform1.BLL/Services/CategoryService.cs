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
        public CategoryService (ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<CategoryResponse> CreateCategory(CategoryRequest request,string lang="en")
        {
            var category = request.Adapt<Category>();
           await _categoryRepository.CreateAsync(category);
            if (request.translations == null || !request.translations.Any() || request.translations.Any(t => t == null))
            {
                throw new ArgumentException("Translations cannot be null or contain null items");
            }
            return category.BuildAdapter().AddParameters("lang", lang).AdaptToType<CategoryResponse>();
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
            var response=categories.BuildAdapter().AddParameters("lang",lang).AdaptToType<List<CategoryResponse>>();
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
            return await _categoryRepository.DeleteAsync(category);


        }
    }
}
