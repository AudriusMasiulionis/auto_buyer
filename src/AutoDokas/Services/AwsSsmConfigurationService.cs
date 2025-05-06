using Amazon;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using AutoDokas.Services.Options;
using Microsoft.Extensions.Options;

namespace AutoDokas.Services;

/// <summary>
/// Service to retrieve configuration values from AWS Systems Manager Parameter Store
/// </summary>
public class AwsSsmConfigurationService
{
    private readonly IAmazonSimpleSystemsManagement _ssmClient;
    private readonly AwsSsmOptions _ssmOptions;

    public AwsSsmConfigurationService(IOptions<AwsSsmOptions> ssmOptions)
    {
        _ssmOptions = ssmOptions.Value;
        _ssmClient = new AmazonSimpleSystemsManagementClient(RegionEndpoint.GetBySystemName("eu-central-1"));
    }

    /// <summary>
    /// Retrieves a parameter value from SSM
    /// </summary>
    /// <param name="parameterName">The name of the parameter to retrieve</param>
    /// <returns>The parameter value, or null if not found</returns>
    public async Task<string?> GetParameterAsync(string parameterName)
    {
        try
        {
            var request = new GetParameterRequest
            {
                Name = parameterName,
                WithDecryption = _ssmOptions.DecryptParameters
            };

            var response = await _ssmClient.GetParameterAsync(request);
            return response.Parameter.Value;
        }
        catch (Exception)
        {
            // Parameter doesn't exist
            return null;
        }
    }
}