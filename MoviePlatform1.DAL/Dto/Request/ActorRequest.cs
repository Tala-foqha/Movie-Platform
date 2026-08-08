using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.DAL.Dto.Request
{
    public class ActorRequest
    {
       
           public List<ActorTranslation> ActorTranslations {  get; set; }

            public IFormFile Image { get; set; }

            public DateTime DateOfBirth { get; set; }
        
    }
}
