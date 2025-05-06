using Amazon;
using Amazon.SimpleEmail;
using AutoDokas.Services.Options;
using Microsoft.Extensions.Options;

namespace AutoDokas.Services;

/// <summary>
/// Implementation of the email service that uses Amazon SES SMTP with IAM role-based authentication
/// </summary>
public class AmazonSesEmailService : IEmailService
{
    private readonly ILogger<AmazonSesEmailService> _logger;
    private const string Region = "eu-central-1";

    public AmazonSesEmailService(
        ILogger<AmazonSesEmailService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Sends an email using SMTP with AWS SES credentials from IAM role
    /// </summary>
    public async Task SendEmailAsync(string from, string to, string subject, string body)
    {
        try
        {
            using var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.GetBySystemName(Region));

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
                _logger.LogInformation("Email sent successfully to {Recipient}", to);
            }
            else
            {
                _logger.LogWarning("Failed to send email to {Recipient}. Status code: {StatusCode}", to, response.HttpStatusCode);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Recipient}", to);
        }
    }
}