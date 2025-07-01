using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.DataProtection;
using YemekhaneApp.Frontend.Components;
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


//-------------------
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
});


builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/root/.aspnet/DataProtection-Keys"));

builder.Services.AddBlazoredSessionStorage();
//// Minimal Authentication (sadece [Authorize] çalýþmasý için)
//builder.Services.AddAuthentication("Cookies")
//    .AddCookie("Cookies", options =>
//    {
//        options.LoginPath = "/login"; 
//        options.AccessDeniedPath = "/access-denied";
//        options.ExpireTimeSpan = TimeSpan.FromDays(30);
//    });

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/login";
    });


builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthorization();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
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
}

app.UseSession();
//app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
