using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.DAL.Dto.Request
{
    public class MovieRequest
    {
        public DateTime ReleaseDate { get; set; }
        public IFormFile MainImage { get; set; }
        public List<IFormFile> MovieImages { get; set; }
        public string AgeRating { get; set; }
        public string Duration { get; set; }

        public List<int> CategoryIds { get; set; }
        public List<int> ActorIds { get; set; }
        public decimal? price { get; set; }

        public List<MovieTranslationRequest> Translations { get; set; }
        public bool IsExclusive { get; set; }


    }
}
