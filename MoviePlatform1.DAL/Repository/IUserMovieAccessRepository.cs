using Microsoft.EntityFrameworkCore.Migrations;
using MoviePlatform1.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.DAL.Repository
{
    public  interface IUserMovieAccessRepository : IGenericRepository<UserMovieAccess>
    {
    }
}
