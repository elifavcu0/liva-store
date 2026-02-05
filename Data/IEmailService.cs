namespace dotnet_store.Models;

public interface IEmailService
{
    Task SendEmailAsync(string email, string subject, string message);
}
