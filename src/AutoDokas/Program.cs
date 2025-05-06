using AutoDokas.Api;
using AutoDokas.Components;
using AutoDokas.Data;
using AutoDokas.Extensions;
using AutoDokas.Services;
using AutoDokas.Services.Options;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using PuppeteerSharp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services
    .AddDbContextFactory<AppDbContext>(opt =>
        opt.UseSqlite(builder.Configuration.GetConnectionString("AppDbContext")))
    .AddLocalization(options => { options.ResourcesPath = "Resources"; });

// Configure AWS SSM options
builder.Services.Configure<AwsSsmOptions>(
    builder.Configuration.GetSection(AwsSsmOptions.SectionName));

// Add AWS SSM configuration services - this will register AmazonSesOptions to be loaded from SSM
builder.Services.AddAwsSsmConfiguration();

// Add data services (CSV reader, etc.)
builder.Services.AddDataServices();

builder.Services.AddScoped<AppDbContext>();

// Register HTML renderer for component rendering in PDFs and emails
builder.Services.AddScoped<HtmlRenderer>();

// Register the PDF service
builder.Services.AddScoped<IPdfService, PdfService>();

// Register the email template factory
builder.Services.AddScoped<IEmailTemplateFactory, RazorEmailTemplateFactory>();

// Register the email notification service
builder.Services.AddScoped<EmailNotificationService>();

// Register the appropriate email service based on the environment
builder.Services.AddEmailServices(builder.Environment);

builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Download the Chromium browser if needed during application startup
await new BrowserFetcher().DownloadAsync();

// Initialize static data in memory
app.InitializeStaticData();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// Map contract API endpoints from the ContractEndpoints class
app.MapContractEndpoints();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();