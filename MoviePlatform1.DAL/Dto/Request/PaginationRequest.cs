using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.DAL.Dto.Request
{
    public class PaginationRequest
    {
        //بأي صفحة هو مثلا بأول صفحة بجيب اول عشرة
        public int Page { get; set; } = 1;
        //كم منتج بكل صفحة
        public int Limit { get; set; }
        public string? Search {  get; set; }
    }
}
