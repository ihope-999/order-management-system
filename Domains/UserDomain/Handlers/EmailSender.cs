using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using project1.Domains.UserDomain.Interfaces;
using project1.Domains.UserDomain.User;

namespace project1.Domains.UserDomain.Handlers
{
    public class EmailSender : Interfaces.IEmailSender

    {

    public EmailSettings _emailSettings { get; set; }

    public EmailSender(IOptions<EmailSettings> emailSettings)  
     { 
        _emailSettings = emailSettings.Value;
     }

    public void sendEmail(string toEmail, string Subject, string Body, string Token, bool enabled = false)
    {

        if(enabled)
            {

                var smtpClient = new SmtpClient(_emailSettings.SmtpHost, _emailSettings.SmtpPort);
                smtpClient.EnableSsl = true;

                var msg = new MailMessage
                {
                    From = new MailAddress(toEmail),
                    Subject = Token,
                    Body = Body

                };
                msg.To.Add(toEmail);
                smtpClient.Send(msg);
            }


        }




    }
}
