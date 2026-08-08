using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.DAL.Models
{
    public class Actor: AuditEntity
    {
        public int Id { get; set; }    
        public string ImageUrl {  get; set; }
        public DateTime DateOfBirth {  get; set; }
        public List<ActorTranslation> ActorTranslations { get; set; }
        public List<MovieActor> MovieActors { get; set; }

    }
}
