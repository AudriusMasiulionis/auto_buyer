namespace AutoDokas.Services;

/// <summary>
/// A fake implementation of the email service that logs emails rather than sending them.
/// This is intended for development and testing purposes.
/// </summary>
public class FakeEmailService : IEmailService
{
    private readonly ILogger<FakeEmailService> _logger;

    public FakeEmailService(ILogger<FakeEmailService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Logs the email instead of actually sending it
    /// </summary>
    public Task SendEmailAsync(string from, string to, string subject, string body)
    {
        _logger.LogInformation("FAKE EMAIL SENT\n" +
                            "From: {From}\n" +
                            "To: {To}\n" +
                            "Subject: {Subject}\n" +
                            "Body: {Body}",
                            from, to, subject, body);

        return Task.CompletedTask;
    }
}