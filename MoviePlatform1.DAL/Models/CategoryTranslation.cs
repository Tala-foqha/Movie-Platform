using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.DAL.Models
{
    public class CategoryTranslation
    {
        public int Id { get; set; }
        //مش رح تكون نل هي القصد منها 
        public String Name { get; set; } = null!;
        //by default equal en
        public String Language { get; set; } = "en";
        public int CategoryId { get; set; }
        public Category Category { get; set; }  

    }
}
