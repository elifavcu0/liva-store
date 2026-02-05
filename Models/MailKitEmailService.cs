using MimeKit;
using Org.BouncyCastle.Security;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace dotnet_store.Models;

public class MailKitEmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public MailKitEmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_configuration["Email:Username"]));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = subject;

        var builder = new BodyBuilder();
        builder.HtmlBody = message;
        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();

        try
        {
            await smtp.ConnectAsync(_configuration["Email:Host"], 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_configuration["Email:Username"], _configuration["Email:Password"]);
            await smtp.SendAsync(email);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Email couldn't be sent : {ex.Message}");
        }
        finally
        {
            await smtp.DisconnectAsync(true);
        }

    }
}