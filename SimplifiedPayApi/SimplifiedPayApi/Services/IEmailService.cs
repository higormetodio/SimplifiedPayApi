using Microsoft.AspNetCore.SignalR.Protocol;

namespace SimplifiedPayApi.Services;

public interface IEmailService
{
    Task SendEmailAsync(IConfiguration configuration, string toName, string toEmail, string subject = "SimplePay Transaction OK",
                        string message = "Your transaction was successful", string fromEmail = "higor.metodio@outlook.com");
}
