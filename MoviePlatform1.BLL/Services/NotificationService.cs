using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MoviePlatform1.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.BLL.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;
        public NotificationService(IEmailSender emailSender, UserManager<ApplicationUser> userManager)
        {
            _emailSender = emailSender;
            _userManager = userManager;
        }

        public async Task NotifyMovieAdded(string movie)
        {
            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                if (!string.IsNullOrEmpty(user.Email))
                {
                    await _emailSender.SendEmail(
                        user.Email,
                        "New Movie Added",
                        $"A new movie has been added:{movie}"
                        );
                }


            }
        }

        public async Task NotifyMovieUpdated(string movie)
        {
            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                if (!string.IsNullOrEmpty(user.Email))
                {
                    await _emailSender.SendEmail(
                        user.Email,
                        "New Movie Added",
                        $"The movie{movie} has been updated "
                        );
                }
            }
        }
    }
}
