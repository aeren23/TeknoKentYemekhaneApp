using Aspire.Hosting.ApplicationModel; // Gerekli Aspire namespace'i
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using YemekhaneApp.Application;
using YemekhaneApp.Persistence;
using YemekhaneApp.Persistence.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.AddSqlServerClient("db");


builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/root/.aspnet/DataProtection-Keys"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPersistenceServices(builder.Configuration.GetConnectionString("YemekhaneDb")); 
builder.Services.AddApplicationRegistration();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// Aspire ile gelen connection string'i kullanmak i�in:
// var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__YemekhaneDb");
var connectionString = builder.Configuration.GetConnectionString("YemekhaneDb") 
        ?? Environment.GetEnvironmentVariable("ConnectionStrings__YemekhaneDb");


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddHostedService<MonthlyDebtJobService>();

// CORS eklemesi
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("EmployeeApiCors", opts =>
    {
        opts.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("EmployeeApiCors"); // CORS middleware

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
