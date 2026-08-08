using Microsoft.AspNetCore.Http;
using MoviePlatform1.DAL.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.DAL.Dto.Request
{
    public class CategoryRequest
    {
        public List<CategoryTranslatiomRequest> translations { get; set; }
        public IFormFile MainImage { get; set; }
    }
}
