using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.DAL.Models
{
    public class Movie: AuditEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime RelaeseDate { get; set; }
        public string MainImage { get; set; }
        public string AgeRating { get; set; }
        public string Duration { get; set; }
        public List<Favorite> Favorites {  get; set; }
        public List<Review> Reviews {  get; set; }
       public List<MovieCategory> MovieCategories {  get; set; }
        public  List<MovieActor> MovieActors { get; set;}

    }
}
