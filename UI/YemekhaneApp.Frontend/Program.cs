using Microsoft.AspNetCore.DataProtection;
using YemekhaneApp.Frontend.Components;
using YemekhaneApp.Frontend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/root/.aspnet/DataProtection-Keys"));


var apiHttpUrl = Environment.GetEnvironmentVariable("SERVICES__API__HTTP__0");
var apiHttpsUrl = Environment.GetEnvironmentVariable("SERVICES__API__HTTPS__0");

var apiBaseUrl = apiHttpsUrl ?? apiHttpUrl;

if (apiBaseUrl == null)
{
    apiBaseUrl = builder.Configuration["ApiUrl"];
}


builder.Services.AddHttpClient<EmployeeService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

builder.Services.AddHttpClient<MealRecordService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
