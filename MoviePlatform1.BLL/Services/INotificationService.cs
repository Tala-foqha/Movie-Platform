using MoviePlatform1.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.BLL.Services
{
    public interface INotificationService
    {
        public Task NotifyMovieAdded(string movie);
        public Task NotifyMovieUpdated(string movie);
    }
}
