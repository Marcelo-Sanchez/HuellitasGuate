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
            message.From = new MailAddress("marcelo23@yopmail.com");
            message.To.Add(email);
            message.Subject = subject;
            message.Body = htmlMessage;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.Huellitas.somee.com", 25);
            client.EnableSsl = false;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("marcelo23@yopmail.com", "123456789Sa");
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
