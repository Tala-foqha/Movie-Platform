using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.DAL.Models
{
    public  class ActorTranslation
    {

        public int Id { get; set; }
        public string Language {  get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ActorId { get; set; }
        public Actor Actor { get; set; }
    }
}
