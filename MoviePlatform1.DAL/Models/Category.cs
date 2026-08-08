using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.DAL.Models
{
    public class Category:AuditEntity
    {
        public int Id { get; set; }
        public string ImageUrl {  get; set; }
        public List<CategoryTranslation> translations { get; set; }


    }
}
