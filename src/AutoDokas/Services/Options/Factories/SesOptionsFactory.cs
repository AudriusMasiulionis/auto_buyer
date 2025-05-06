using AutoDokas.Services.Options;
using Microsoft.Extensions.Options;

namespace AutoDokas.Services.Options.Factories;

/// <summary>
/// Options factory that loads Amazon SES values from SSM
/// </summary>
public class SesOptionsFactory : IOptionsFactory<AmazonSesOptions>
{
    private readonly AwsSsmConfigurationService _ssmService;
    private readonly Dictionary<string, string> parameterNames = new()
    {
        { "UserName", "/autodokasapp/smtp/username" },
        { "Password", "/autodokasapp/smtp/password" },
        // { "Region", "/autodokasapp/ses/region" },
        // { "SourceEmail", "/autodokasapp/ses/sourceEmail" },
        // { "BaseUrl", "/autodokasapp/ses/baseUrl" },
        // { "UseDmarc", "/autodokasapp/ses/useDmarc" }
    };

    public SesOptionsFactory(AwsSsmConfigurationService ssmService)
    {
        _ssmService = ssmService;
    }

    public AmazonSesOptions Create(string name)
    {
        // Dictionary<string, Task<string?>> taskMap = [];

        // foreach (var parameterName in parameterNames)
        // {
        //     taskMap.Add(parameterName.Key, _ssmService.GetParameterAsync(parameterName.Value));
        // }

        // Task.WaitAll(taskMap.Values);

        return new()
        {
            // UserName = taskMap["UserName"].Result ?? string.Empty,
            // Password = taskMap["Password"].Result ?? string.Empty,
            // Region = taskMap["Region"].Result ?? "eu-west-1",
            // SourceEmail = taskMap["SourceEmail"].Result,
            // BaseUrl = taskMap["BaseUrl"].Result,
            // UseDmarc = taskMap["UseDmarc"].Result == "true"
        };

    }
}