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
    /// <param name="to">Recipient email address</param>
    /// <param name="subject">Email subject</param>
    /// <param name="body">Email body content</param>
    /// <returns>A task that represents the asynchronous send operation</returns>
    Task SendEmailAsync(string to, string subject, string body);
    
    /// <summary>
    /// Sends a contract notification email to the buyer
    /// </summary>
    /// <param name="to">Buyer's email address</param>
    /// <param name="contract">The vehicle contract with details to include in the email</param>
    /// <returns>A task that represents the asynchronous send operation</returns>
    Task SendContractNotificationAsync(string to, VehicleContract contract);
}