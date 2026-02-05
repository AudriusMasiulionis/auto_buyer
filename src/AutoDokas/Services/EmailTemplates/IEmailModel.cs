using AutoDokas.Data.Models;

namespace AutoDokas.Services.EmailTemplates;

public interface IEmailModel
{
    string Subject { get; }
    VehicleContract Contract { get; set; }
    string BaseUrl { get; set; }
}
