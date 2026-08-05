using MoviePlatform1.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.DAL.Models
{
    public class MovieTranslation
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string Language { get; set; } = "en";
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
