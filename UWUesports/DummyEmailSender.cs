using Microsoft.AspNetCore.Identity.UI.Services;

public class DummyEmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // Tutaj nic nie robimy lub możesz zalogować do konsoli, że mail miał być wysłany
        return Task.CompletedTask;
    }
}
