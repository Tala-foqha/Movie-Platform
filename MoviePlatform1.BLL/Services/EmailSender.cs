using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.BLL.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmail(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("foqhat835@gmail.com", "your password")
            };

            return client.SendMailAsync(
                new MailMessage(from: "foqhat835@gmail.com",
                                to: email,
                                subject,
                                message

                                )
                { IsBodyHtml = true });
          
        }
    }

    }

