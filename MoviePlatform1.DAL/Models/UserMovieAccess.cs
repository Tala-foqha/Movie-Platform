using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.DAL.Models
{
    public class UserMovieAccess
    {
       
            public int Id { get; set; }

            public string UserId { get; set; }
            public ApplicationUser User { get; set; }

            public int MovieId { get; set; }
            public Movie Movie { get; set; }

            public bool HasAccess { get; set; }

        }
    }

