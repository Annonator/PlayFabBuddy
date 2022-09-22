using PlayFabBuddy.Infrastructure;
using PlayFabBuddy.Lib;
using PlayFabBuddy.UI.Data;

var builder = WebApplication.CreateBuilder(args);

var customConfigBuilder = new ConfigurationBuilder();
customConfigBuilder.AddJsonFile("settings.json", false, true);
customConfigBuilder.AddJsonFile("local.settings.json", true, true);

var customConfig = customConfigBuilder.Build();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<IConfiguration>(customConfig);
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<PlayFabService>();
builder.Services.AddLibrary(customConfig);
builder.Services.AddInfrastructure(customConfig);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
