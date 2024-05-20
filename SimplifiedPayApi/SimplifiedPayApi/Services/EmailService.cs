
using MailKit.Net.Smtp;
using MimeKit;

namespace SimplifiedPayApi.Services;

public class EmailService : IEmailService
{
    public async Task SendEmailAsync(IConfiguration configuration, string toName, string toEmail, string subject = "SimplePay Transaction OK", 
                               string message = "Your transaction was successful", string fromEmail = "higor.metodio@outlook.com")
    {
        var host = configuration.GetSection("SMTP").GetValue<string>("Host");
        var port = configuration.GetSection("SMTP").GetValue<int>("Port");
        var userName = configuration.GetSection("SMTP").GetValue<string>("UserName");
        var passWord = configuration.GetSection("SMTP").GetValue<string>("Password");

        var mail = new MimeMessage();
        mail.From.Add(new MailboxAddress(userName, fromEmail));
        mail.To.Add(new MailboxAddress(toName, toEmail));
        mail.Subject = subject;
        mail.Body = new TextPart("html")
        {
            Text = message
        };

        using (var smptClient = new SmtpClient())
        {
            await smptClient.ConnectAsync(host, port);
            await smptClient.AuthenticateAsync(userName, passWord);
            await smptClient.SendAsync(mail);
            await smptClient.DisconnectAsync(true);
        }
    }
}
