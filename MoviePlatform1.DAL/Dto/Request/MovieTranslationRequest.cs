using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.DAL.Dto.Request
{
    public class MovieTranslationRequest
    {
        public string Language { get; set; }   // مثال: ar, en
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
