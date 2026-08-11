using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.DAL.Dto.Response
{
    public  class CartResponse
    {
        public int movieId { get; set; }
        public decimal Price { get; set; }
        public string MovieName { get; set; }
        public string ProductImage { get; set; }
    }
}
