using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using DotNet.Testcontainers.Images;

namespace AutoDokas.Tests.UI;

[TestFixture]
public class TestBase : PageTest
{
    private readonly IContainer _blazorContainer;
    private IFutureDockerImage _dockerImage;
    protected string BaseUrl;

    private const int MappedPort = 8080; // Port to bind the container to

    public TestBase()
    {
        // Create container configuration using our Dockerfile
        _blazorContainer = new ContainerBuilder()
            .WithImage("autodokasapp:latest")
            .WithName($"autodokasapp-test-{Guid.NewGuid()}")
            .WithPortBinding(MappedPort, 80)
            .WithEnvironment(new Dictionary<string, string>
            {
                ["ASPNETCORE_ENVIRONMENT"] = "Development",
                ["ConnectionStrings__AppDbContext"] = "Data Source=app.db",
                ["ASPNETCORE_URLS"] = "http://+:80"
            })
            .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(80)))
            .Build();
    }

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        // Build the Docker image
        _ = await DockerImageHelper.BuildImageAsync();
        
        // Start the container
        await _blazorContainer.StartAsync();
        
        BaseUrl = $"http://localhost:{MappedPort}";
        
        // Verify that the container is responding by making a simple HTTP request
        using var httpClient = new HttpClient();
        var response = await httpClient.GetAsync(BaseUrl);
        
        Console.WriteLine($"Blazor app is running at {BaseUrl} - Status: {response.StatusCode}");
    }

    [SetUp]
    public async Task Setup()
    {
        // Initialize browser context with specific options if needed
        await Context.Tracing.StartAsync(new()
        {
            Screenshots = true,
            Snapshots = true
        });
    }

    [TearDown]
    public async Task Teardown()
    {
        // Capture trace for failed tests
        if (TestContext.CurrentContext.Result.Outcome.Status != NUnit.Framework.Interfaces.TestStatus.Passed)
        {
            string testName = TestContext.CurrentContext.Test.Name;
            await Context.Tracing.StopAsync(new()
            {
                Path = $"trace-{testName}.zip"
            });
        }
        else
        {
            await Context.Tracing.StopAsync();
        }
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        // Stop and remove the container when tests are done
        await _blazorContainer.StopAsync();
        await _blazorContainer.DisposeAsync();
    }
}