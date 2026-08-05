using KASHOP.DAL.Dto.Request;
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
    }
}
