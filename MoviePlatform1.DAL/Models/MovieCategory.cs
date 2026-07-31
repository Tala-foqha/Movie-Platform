using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.DAL.Models
{
    [PrimaryKey(nameof(MovieId), nameof(CategoryId))]

    public class MovieCategory
    {
        public int MovieId {  get; set; }
        public Movie Movie { get; set; }
        public int CategoryId {  get; set; }
        public Category Category { get; set; }
    }
}
