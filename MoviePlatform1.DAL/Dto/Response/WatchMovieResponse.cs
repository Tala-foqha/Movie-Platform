using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.DAL.Dto.Response
{
   
        public class WatchMovieResponse
        {
            public bool CanWatch { get; set; }
            public string Message { get; set; }
            public string? MovieUrl { get; set; }
        }
   
}
