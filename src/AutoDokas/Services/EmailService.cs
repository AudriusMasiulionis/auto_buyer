using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Options;

namespace AutoDokas.Services;

public interface IEmailService
{
    Task SendEmailAsync(string from, string to, string subject, string html, Dictionary<string, string>? vars = null);
}

public class FakeEmailService(ILogger<FakeEmailService> logger) : IEmailService
{
    public Task SendEmailAsync(string from, string to, string subject, string html, Dictionary<string, string>? vars = null)
    {
        var varsLog = vars != null ? string.Join(", ", vars.Select(kv => $"{kv.Key}={kv.Value}")) : "(none)";
        logger.LogInformation("FAKE EMAIL SENT\nFrom: {From}\nTo: {To}\nSubject: {Subject}\nVars: {Vars}\nHtml: {Html}",
            from, to, subject, varsLog, html);
        return Task.CompletedTask;
    }
}

public class EmailLabsOptions
{
    public const string SectionName = "EmailLabs";

    public string AppKey { get; set; } = "";
    public string SecretKey { get; set; } = "";
    public string SmtpAccount { get; set; } = "";
}

public class EmailLabsEmailService(
    IHttpClientFactory httpClientFactory,
    IOptions<EmailLabsOptions> options,
    ILogger<EmailLabsEmailService> logger) : IEmailService
{
    private const string ApiUrl = "https://api.emaillabs.net.pl/api/sendmail_templates";

    public async Task SendEmailAsync(string from, string to, string subject, string html, Dictionary<string, string>? vars = null)
    {
        var opts = options.Value;

        var formData = new Dictionary<string, string>
        {
            ["smtp_account"] = opts.SmtpAccount,
            ["from"] = from,
            ["subject"] = subject,
            ["html"] = html,
            [$"to[{to}]"] = ""
        };

        if (vars != null)
        {
            foreach (var (key, value) in vars)
            {
                formData[$"to[{to}][vars][{key}]"] = value;
            }
        }

        using var client = httpClientFactory.CreateClient();
        var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{opts.AppKey}:{opts.SecretKey}"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

        var content = new FormUrlEncodedContent(formData);
        var response = await client.PostAsync(ApiUrl, content);

        if (response.IsSuccessStatusCode)
        {
            logger.LogInformation("Email sent successfully to {Recipient}", to);
        }
        else
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            logger.LogError("Failed to send email to {Recipient}. Status: {StatusCode}, Response: {Response}",
                to, response.StatusCode, responseBody);
        }
    }
}
