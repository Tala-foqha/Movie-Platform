using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.DAL.Dto.Response
{
    public class MovieResponse
    {
        public int Id { get; set; }


        //public string UserCreated { get; set; }
        public string Name { get; set; }
        public string AgeRating { get; set; }


        public string MainImage { get; set; }
        public List<string> Images { get; set; }
        public string movieUrl {  get; set; }
        public bool IsExclusive { get; set; }

    }
}
