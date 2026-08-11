using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.DAL.Dto.Request
{
    public class MovieFiltterRequest:PaginationRequest
    {
        //لانه الداتا الي رح ترجع ممكن اعمل عليها بجنيشن
        public int? CategoryId { get; set; }
        public bool? IsExclusive { get; set; }
        public int? MaxPrice { get; set; }
        public int? MinPrice { get; set; }
    }
}
