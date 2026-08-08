using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.DAL.Dto.Response
{
    public class FavoriteResponse
    {
        public string UserName { get; set; }
        public int MovieId { get; set; }
        public int FavId { get; set; }

        public string MovieTitle { get; set; }

        public string CreatedAt { get; set; }
        public string MainImage{get;set;}

    }
}
