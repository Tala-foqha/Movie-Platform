using Microsoft.EntityFrameworkCore;

namespace MoviePlatform1.DAL.Models
{
    [PrimaryKey(nameof(MovieId), nameof(UserId))]
    public class Cart
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

    }
}