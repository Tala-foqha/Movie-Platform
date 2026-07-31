using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.DAL.Models
{
    public class Favorite
    {
        public int FavoriteId { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public Movie Movie { get; set; }
        public int movieId {  get; set; }

    }
}
