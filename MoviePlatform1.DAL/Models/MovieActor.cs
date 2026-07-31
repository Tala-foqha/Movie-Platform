using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.DAL.Models
{
    [PrimaryKey(nameof(MovieId), nameof(ActorId))]
    public class MovieActor
    {
        public int MovieId {  get; set; }
        public int ActorId {  get; set; }
        public Movie Movie { get; set; }
        public Actor Actor { get; set; }
    }
}
