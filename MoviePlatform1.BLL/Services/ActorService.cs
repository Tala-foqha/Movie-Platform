using Mapster;
using MoviePlatform1.DAL.Dto.Request;
using MoviePlatform1.DAL.Dto.Response;
using MoviePlatform1.DAL.Models;
using MoviePlatform1.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.BLL.Services
{
    public class ActorService : IActorService
    {
        private readonly IFileService _fileService;
        private readonly IActorRepository _actorRepository;
        public ActorService(IFileService fileService,IActorRepository actorRepository)
        {

            _fileService = fileService;
            _actorRepository = actorRepository;
        }
        public async Task<ActorResponse> CreateActorAsync(ActorRequest request)
        {
            var actor = request.Adapt<Actor>();
            if (request.Image != null) { 
                var imagePath = await _fileService.UploadAsync(request.Image);

            if (string.IsNullOrEmpty(imagePath))
            {
                throw new Exception("Image upload failed");
            }

            actor.ImageUrl = imagePath;
        }
     var response= await  _actorRepository.CreateAsync(actor);
            return response.Adapt<ActorResponse>();

    }

        public async Task<List<ActorResponse>> GetAllActors()
        {
            var actors = await _actorRepository.GetAllAsync(
                c=>c.Status==EntityStatus.Active,
                new string[]
                {
                    nameof(Actor.ActorTranslations),
                    nameof(Actor.CreateBy)
                });
            return actors.Adapt<List<ActorResponse>>();


        }
        public async Task<ActorResponse?> GetActor(Expression<Func<Actor, bool>> filter)
        {
            var actor = await _actorRepository.Getone(filter, new string[]
            {
                nameof(Actor.ActorTranslations),
                  nameof(Actor.CreateBy)
            });
            return actor.Adapt<ActorResponse>();
        }
        public async Task<bool> DeleteActor(int id)
        {
            var category = await _actorRepository.Getone(c => c.Id == id);
            if (category == null)
            {
                return false;

            }
            _fileService.Delete(category.ImageUrl);
            return await _actorRepository.DeleteAsync(category);


        }
    }
}
