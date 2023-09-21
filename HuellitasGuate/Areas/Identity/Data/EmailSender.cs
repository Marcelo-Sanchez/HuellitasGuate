using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;

namespace HuellitasGuate.Areas.Identity.Data
{
    public class EmailSender : IEmailSender
    {

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress("wmsanchez11@hotmail.com");
            message.To.Add(email);
            message.Subject = subject;
            message.Body = htmlMessage;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.office365.com", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("wmsanchez11@hotmail.com", "Seguridad2023$");
       
            try
            {
                client.Send(message);
            }
            catch (Exception e)
            {
                var error = e.Message;
                var stacktrace = e.StackTrace;
            }
            finally
            {

            }
            return Task.CompletedTask;
        }
    }

}
