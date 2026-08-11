using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.DAL.Dto.Response
{
    public class OrderItemResponse
    {
        public int MovieId { get; set; }
        public string MovieName { get; set; }
        public int UnitPrice { get; set; }
    }
}
