using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.BLL.Services
{
    internal interface IEmailSender
    {
        public Task SendEmail(string email,string subject,string message);
    }
}
