using System.Net;
using System.Net.Mail;
using System.Text;

namespace Mag.Services
{
    public class EmailService
    {
        public Task Execute(string UserEmail,string Body,string Subject)
        {
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 1000000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("sibou.mag@gmail.com", "reip apre ygbh jcaz");
            MailMessage message = new MailMessage("sibou.mag@gmail.com", UserEmail,Subject,Body);
            message.IsBodyHtml = true;
            message.BodyEncoding = UTF8Encoding.UTF8;
            message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
            client.Send(message);
            return Task.CompletedTask;
        }
    }
}
