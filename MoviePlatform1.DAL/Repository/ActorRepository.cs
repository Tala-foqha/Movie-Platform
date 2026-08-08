using MoviePlatform1.DAL.Data;
using MoviePlatform1.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.DAL.Repository
{
    public class ActorRepository : GenericRepository<Actor>, IActorRepository
    {
        public ActorRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
