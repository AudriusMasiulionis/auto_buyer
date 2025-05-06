using AutoDokas.Data.Models;

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