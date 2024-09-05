using Microsoft.EntityFrameworkCore;
using RecordingApp.Models;
using RecordingApp.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("No connection string was found");

builder.Services.AddDbContext<AppDbContext>(options =>
  options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton(new TranscriptionService("87fe47eaa8b6482093730fa1584308ba"));

builder.Services.AddSingleton<SpeechRecognitionService>(provider =>
{
    // Replace with your actual API key and URL, or load from configuration
    var apiKey = "";
    var apiUrl = "https://api.openai.com/v1/audio/transcriptions";
    return new SpeechRecognitionService(apiKey, apiUrl);
});

builder.Services.AddSingleton<RecordingApp.Services.Azure>(provider =>
    new RecordingApp.Services.Azure("7afac6b1c63a45438199a868325db039", "eastus"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
