using System.Net.Mail;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace ObaGroup.Utility;

public class EmailSender:IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var body = $@"
    <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    font-size: 14px;
                    line-height: 1.5;
                    margin: 0;
                    padding: 0;
                }}
                h1 {{
                    font-size: 28px;
                    margin: 0 0 10px;
                }}
                p {{
                    margin: 0 0 20px;
                }}
                a {{
                    color: #0078d4;
                    text-decoration: none;
                }}
            </style>
        </head>
        <body>
            <h1>Example Email</h1>
            <p>Hello,</p>
            <p>This is an example email with some basic styling. You can customize the HTML markup to fit your needs.</p>
            <p>Regards,</p>
            {htmlMessage}
            <p>John Doe</p>
        </body>
    </html>
";
        var emailToSend = new MimeMessage();
        emailToSend.From.Add(MailboxAddress.Parse("obagroup422@gmail.com"));
        emailToSend.To.Add(MailboxAddress.Parse(email));
        emailToSend.Subject = subject;
        emailToSend.Body =new TextPart(MimeKit.Text.TextFormat.Html){Text = body};

        
        using (var emailClient = new SmtpClient())
        {
            emailClient.Connect("smtp.gmail.com",587,MailKit.Security.SecureSocketOptions.StartTls);
            emailClient.Authenticate("obagroup422@gmail.com","uufbhrpmrazrqxkn");
            emailClient.Send(emailToSend);
            emailClient.Disconnect(true);

        }
        return Task.CompletedTask;
    }
}