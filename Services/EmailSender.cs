using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace GameShop.Services;

public class EmailSender : IEmailSender
{
    private readonly string _smtpServer = "smtp.gmail.com"; // Setează serverul SMTP
    private readonly string _smtpUser = "neaca.radu309@gmail.com";
    private readonly string _smtpPass = "bpsm pszk ymcs ddqu"; 
    private readonly int _smtpPort = 587; 
    
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_smtpUser),
            Subject = subject,
            Body = message,
            IsBodyHtml = true
        };

        mailMessage.To.Add(email);

        using (var smtpClient = new SmtpClient(_smtpServer, _smtpPort))
        {
            smtpClient.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
            smtpClient.EnableSsl = true;

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}