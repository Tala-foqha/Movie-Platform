using Microsoft.EntityFrameworkCore;
using MoviePlatform1.DAL.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.BLL.Extentions
{
    /*
     page   limit   skip
      1       5      0
      2       5      5
      3       5     10




    */
    public static class  PaginationExtentions
    {
        //بالاكتنشن نستخدم ذس 
        //كويربل نعمل ع الداتا وهمي بالدات بيس باجينيشن عدها نرجعهم ع جهاز الوزر
        //qusery لسا ما تنفذت بدنا نكمل عليها السكيب وهاي الشغلات
        public static async Task<PaginationResponse<T>>ToPaginationasync<T>(this IQueryable<T>query,int Page,int Limit)
        {
            var totalCount = await query.CountAsync();
            //take بتجيب عدد معين حسب الرقم الي بدي اياه
            //skip عشان يفشق عن كم 
            var data = await query.Skip((Page - 1) * Limit).Take(Limit).ToListAsync();
            // لما نقله تو ليست هيك رجعنا الداتا جاهزة
            return new PaginationResponse<T>
            {
                Data = data,
                TotalCount = totalCount,
                Page = Page,
                Limit = Limit,
                

            };
        }
    }
}
