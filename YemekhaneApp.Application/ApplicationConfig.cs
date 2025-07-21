using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using YemekhaneApp.Application.Services.Auth;
using YemekhaneApp.Application.Services;
using YemekhaneApp.Application.Services.Token;

namespace YemekhaneApp.Application
{
    public static class ApplicationLayerConfig
    {
        public static void AddApplicationRegistration(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            var assembly = Assembly.GetExecutingAssembly();
            services.AddAutoMapper(assembly);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        }
    }
}
