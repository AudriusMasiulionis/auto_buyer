using Amazon.SimpleEmail;

namespace AutoDokas.Services;


/// <summary>
/// Interface for email service operations
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends an email to the recipient
    /// </summary>
    /// <param name="from">Sender email address</param>
    /// <param name="to">Recipient email address</param>
    /// <param name="subject">Email subject</param>
    /// <param name="body">Email body content</param>
    /// <returns>A task that represents the asynchronous send operation</returns>
    Task SendEmailAsync(string from, string to, string subject, string body);
}

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

/// <summary>
/// Configuration options for Amazon SES email service
/// </summary>
public class AmazonSesOptions
{
    /// <summary>
    /// The section name in the configuration file
    /// </summary>
    public const string SectionName = "AWS";
    
    /// <summary>
    /// AWS region name (e.g., "eu-west-1")
    /// </summary>
    public string Region { get; set; } = "eu-west-1";
}

/// <summary>
/// Implementation of the email service that uses Amazon SES SMTP with IAM role-based authentication
/// </summary>
public class AmazonSesEmailService(
    ILogger<AmazonSesEmailService> logger) : IEmailService
{
    /// <summary>
    /// Sends an email using SMTP with AWS SES credentials from IAM role
    /// </summary>
    public async Task SendEmailAsync(string from, string to, string subject, string body)
    {
        try
        {
            using var client = new AmazonSimpleEmailServiceClient();

            // Create the email content
            var request = new Amazon.SimpleEmail.Model.SendEmailRequest
            {
                Source = from,
                Destination = new Amazon.SimpleEmail.Model.Destination
                {
                    ToAddresses = [to]
                },
                Message = new Amazon.SimpleEmail.Model.Message
                {
                    Subject = new Amazon.SimpleEmail.Model.Content(subject),
                    Body = new Amazon.SimpleEmail.Model.Body
                    {
                        Html = new Amazon.SimpleEmail.Model.Content
                        {
                            Charset = "UTF-8",
                            Data = body
                        }
                    }
                },
                ReturnPath = from
            };

            // Send the email
            var response = await client.SendEmailAsync(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                logger.LogInformation("Email sent successfully to {Recipient}", to);
            }
            else
            {
                logger.LogWarning("Failed to send email to {Recipient}. Status code: {StatusCode}", to, response.HttpStatusCode);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send email to {Recipient}", to);
        }
    }
}