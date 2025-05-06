using System;

namespace AutoDokas.Services.Options;

/// <summary>
/// Configuration options for AWS Systems Manager Parameter Store
/// </summary>
public class AwsSsmOptions
{
    /// <summary>
    /// The section name in the configuration file
    /// </summary>
    public const string SectionName = "AwsSsm";

    /// <summary>
    /// AWS region where the parameters are stored
    /// </summary>
    public string Region { get; set; } = "eu-central-1";

    /// <summary>
    /// Parameter path for SMTP username
    /// </summary>
    public string UsernameParam { get; set; } = "/autodokasapp/smtp/username";

    /// <summary>
    /// Parameter path for SMTP password
    /// </summary>
    public string PasswordParam { get; set; } = "/autodokasapp/smtp/password";

    /// <summary>
    /// Parameter path for SMTP host
    /// </summary>
    public string HostParam { get; set; } = "/autodokasapp/smtp/host";

    /// <summary>
    /// Parameter path for SMTP port
    /// </summary>
    public string PortParam { get; set; } = "/autodokasapp/smtp/port";

    /// <summary>
    /// Whether to decrypt SecureString parameters
    /// </summary>
    public bool DecryptParameters { get; set; } = true;
}