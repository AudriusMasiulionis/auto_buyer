using Microsoft.Playwright;
using System;
using System.Collections.Generic;

namespace AutoDokas.Tests.UI;

public static class PlaywrightConfig
{
    /// <summary>
    /// Default Playwright browser options to use across tests
    /// </summary>
    public static BrowserNewContextOptions DefaultContextOptions => new()
    {
        ViewportSize = new ViewportSize
        {
            Width = 1920,
            Height = 1080
        },
        RecordVideoDir = "videos/",
        AcceptDownloads = true,
        HasTouch = false,
        Locale = "en-US",
        TimezoneId = "Europe/Vilnius", // Change to your local timezone if needed
        HttpCredentials = GetHttpCredentials()
    };

    /// <summary>
    /// Returns credentials for HTTP authentication if needed
    /// </summary>
    private static HttpCredentials GetHttpCredentials()
    {
        // Add these only if your site requires HTTP authentication
        string username = Environment.GetEnvironmentVariable("AUTH_USERNAME");
        string password = Environment.GetEnvironmentVariable("AUTH_PASSWORD");

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            return new HttpCredentials
            {
                Username = username,
                Password = password
            };
        }

        return null;
    }
}