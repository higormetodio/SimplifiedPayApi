using Microsoft.AspNetCore.SignalR.Protocol;

namespace SimplifiedPayApi.Services;

public interface IEmailService
{
    Task SendEmailAsync(IConfiguration configuration, string toName, string toEmail, string subject = "SimplePay Transaction OK",
                        string message = "You received a transaction through SimplePay", string fromEmail = "contatct@simplepay.com.br");
}
