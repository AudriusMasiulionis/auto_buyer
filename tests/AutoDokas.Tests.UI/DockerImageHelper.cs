using DotNet.Testcontainers.Images;
using DotNet.Testcontainers.Builders;

namespace AutoDokas.Tests.UI;

public static class DockerImageHelper
{
    private const string ImageName = "autodokasapp:latest";
    
    public static async Task<IFutureDockerImage> BuildImageAsync()
    {
        Console.WriteLine($"Building Docker image '{ImageName}'...");
        
        // Find the Dockerfile directory
        var dockerfilePath = Path.Combine("src", "AutoDokas");
        
        // Build the Docker image using the Testcontainers API
        var futureImage = new ImageFromDockerfileBuilder()
            .WithName(ImageName)
            .WithDockerfileDirectory(CommonDirectoryPath.GetSolutionDirectory(), dockerfilePath)
            .WithDockerfile("Dockerfile")
            .WithDeleteIfExists(true)
            .Build();
        
        // Create the image
        await futureImage.CreateAsync();
        
        Console.WriteLine($"Docker image '{ImageName}' built successfully.");
        return futureImage;
    }
}