using AutoDokas.Components;
using AutoDokas.Data;
using AutoDokas.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services
    .AddDbContextFactory<AppDbContext>(opt =>
        opt.UseSqlite(builder.Configuration.GetConnectionString("AppDbContext")))
    .AddLocalization(options => { options.ResourcesPath = "Resources"; });

// Register the email service
builder.Services.AddScoped<IEmailService, FakeEmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();