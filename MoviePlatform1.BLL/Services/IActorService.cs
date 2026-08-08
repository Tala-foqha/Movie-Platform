using MoviePlatform1.DAL.Dto.Request;
using MoviePlatform1.DAL.Dto.Response;
using MoviePlatform1.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.BLL.Services
{
    public interface IActorService
    {
        public Task<ActorResponse> CreateActorAsync(ActorRequest request);
        public Task<List<ActorResponse>> GetAllActors();
        public  Task<ActorResponse?> GetActor(Expression<Func<Actor, bool>> filter);
        public  Task<bool> DeleteActor(int id);

    }
}
