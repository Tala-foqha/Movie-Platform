using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.DAL.Dto.Response
{
    public class ReviewResponse
    {
        public int Id { get; set; }

        public string Comment { get; set; }

        public string UserName { get; set; }

        public int MovieId { get; set; }

        public string MovieTitle { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
