using Api.Contracts;
using FastEndpoints;
using FastEndpoints.Swagger;
using Api.Jobs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddEnvironmentVariables();
builder.Services
    .AddDbContext<ApplicationDbContext>(opt =>
    {
        var dbFolderPath = Path.Combine(Environment.CurrentDirectory, "Data");
        if (Directory.Exists(dbFolderPath) == false)
            Directory.CreateDirectory(dbFolderPath);
        var dbPath = Path.Combine(dbFolderPath, "AppDatabase.db");
        opt.UseSqlite($"Data Source={dbPath}");
    })
    .AddFastEndpoints()
    .AddJobQueues<JobRecord, JobStorageProvider>()
    .AddEndpointsApiExplorer()
    .SwaggerDocument();

var app = builder.Build();

// Apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

app.UseFastEndpoints()
    .UseJobQueues()
    .UseSwaggerGen();

app.Run();