using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.DAL.Dto.Request
{
    public class ReviewRequest
    {
       
            public int MovieId { get; set; }

            public string Comment { get; set; }
        
    }
}
