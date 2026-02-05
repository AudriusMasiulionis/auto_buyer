using AutoDokas.Api;
using AutoDokas.Components;
using AutoDokas.Data;
using AutoDokas.Extensions;
using AutoDokas.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Localization;
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

// Configure supported cultures
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "lt", "en" };
    options.SetDefaultCulture("lt")
           .AddSupportedCultures(supportedCultures)
           .AddSupportedUICultures(supportedCultures);
    
    options.RequestCultureProviders.Clear();
    options.RequestCultureProviders.Add(new QueryStringRequestCultureProvider());
    options.RequestCultureProviders.Add(new CookieRequestCultureProvider());
    // Remove AcceptLanguageHeaderRequestCultureProvider to prevent browser language override
    // options.RequestCultureProviders.Add(new AcceptLanguageHeaderRequestCultureProvider());
    
    // Set fallback behavior - if no culture provider matches, use "lt"
    options.FallBackToParentCultures = false;
    options.FallBackToParentUICultures = false;
});

// Add data services (CSV reader, etc.)
builder.Services.AddDataServices();

builder.Services.AddScoped<AppDbContext>();

// Register HTML renderer for component rendering in PDFs and emails
builder.Services.AddScoped<HtmlRenderer>();

// Register the PDF service
builder.Services.AddScoped<PdfService>();

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

// Apply pending EF Core migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

// Download the Chromium browser if needed during application startup
await new BrowserFetcher().DownloadAsync();

// Initialize static data in memory
app.InitializeStaticData();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRequestLocalization();
app.UseAntiforgery();

// Map contract API endpoints from the ContractEndpoints class
app.MapContractEndpoints();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();