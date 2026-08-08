using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using MoviePlatform1.DAL.Dto.Request;
using MoviePlatform1.DAL.Dto.Response;
using MoviePlatform1.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.BLL.Services
{
    public interface  ICategoryService
    {
       public Task<CategoryResponse> CreateCategory(CategoryRequest request, string lang = "en");
        public Task<List<CategoryResponse>> GetAllCategories(string lang = "en");
       public  Task<CategoryResponse?> GetCategory(Expression<Func<Category, bool>> filter);
      public Task<bool> DeleteCategory(int id);
        public Task<bool>UpdateCategory(int id, CategoryUpdateRequest request);



    }
}
