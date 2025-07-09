using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.DataProtection;
using YemekhaneApp.Frontend.Components;
using YemekhaneApp.Frontend.Helpers;
using YemekhaneApp.Frontend.Security;
using YemekhaneApp.Frontend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Circuit options for detailed errors
builder.Services.Configure<CircuitOptions>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.DetailedErrors = true;
    }
});



builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/root/.aspnet/DataProtection-Keys"));





builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomAuthenticationStateComponent>();
builder.Services.AddScoped<CookieHelper>();
builder.Services.AddScoped<UserAgentHelper>();


builder.Services.AddAuthentication()
    .AddScheme<CustomOptions, CustomAuthenticationHandler>("CustomAuth", options => { });

builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthenticationStateProvider>());

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<JwtAuthTokenMessageHandler>();


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
}).AddHttpMessageHandler<JwtAuthTokenMessageHandler>();

builder.Services.AddHttpClient<MealRecordService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
}).AddHttpMessageHandler<JwtAuthTokenMessageHandler>();

builder.Services.AddHttpClient<UserDebtService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
}).AddHttpMessageHandler<JwtAuthTokenMessageHandler>();

builder.Services.AddHttpClient<ExtraService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
}).AddHttpMessageHandler<JwtAuthTokenMessageHandler>();

builder.Services.AddHttpClient<AuthService>(client =>
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
    app.UseHttpsRedirection();
}


app.UseStaticFiles();
app.UseAntiforgery();


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
