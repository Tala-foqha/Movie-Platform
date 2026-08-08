using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.DAL.Dto.Response
{
    public class ActorResponse
    {
        public int ActorId {  get; set; }
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public string MainImage {  get; set; }
    }
}
