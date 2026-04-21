using System.Net;
using System.Net.Mail;

namespace Blog.Services;

public class EmailService
{
    public bool Send(
        string toName,
        string toEmail,
        string subject,
        string body,
        string fromName = "Equipe balta.io",
        string fromEmail = "edsonr065@gmail.com")
    {
        var smtp = new SmtpClient(Configuration.Smtp.Host, Configuration.Smtp.Port);
        
        smtp.Credentials = new NetworkCredential(Configuration.Smtp.Username, Configuration.Smtp.Password);
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtp.EnableSsl = true;

        var mail = new MailMessage();

        mail.From = new MailAddress(fromEmail, fromName);
        mail.To.Add(new MailAddress(toEmail, toName));
        mail.Subject = subject;
        mail.Body = body;
        mail.IsBodyHtml = true;

        try
        {
        smtp.Send(mail);
        return true;
        }
        catch (System.Exception)
        {
        
        throw;
        }
    }


}