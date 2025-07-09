using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using YemekhaneApp.Application.DTOs.Auth;
using YemekhaneApp.Application.Interfaces;
using YemekhaneApp.Application.Services.Auth;
using YemekhaneApp.Application.Services.Token;
using YemekhaneApp.Domain.Entities;

namespace YemekhaneApp.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> AuthenticateAsync(string password, string userAgent)
        {
            var systemPassword = _configuration["SystemPassword"];

            if (password != systemPassword)
                throw new UnauthorizedAccessException("Şifre hatalı.");

            // Güvenli: Kayıt yoksa null döner, exception fırlatmaz
            var existingDevice = await _unitOfWork.GetRepository<TrustedDevice>()
                .GetSingleOrDefaultAsync(x => x.UserAgent == userAgent);

            if (existingDevice == null)
            {
                var device = new TrustedDevice
                {
                    UserAgent = userAgent,
                    CreatedAt = DateTime.UtcNow,
                };

                await _unitOfWork.GetRepository<TrustedDevice>().AddAsync(device);
                await _unitOfWork.SaveAsync();
            }

            var token = _tokenService.GenerateToken("defaultuser");

            return new AuthResponseDto { Token = token };
        }

        public async Task<bool> IsTrustedDeviceAsync(string userAgent)
        {
            var exists = await _unitOfWork.GetRepository<TrustedDevice>()
                .AnyAsync(x => x.UserAgent == userAgent);
            return exists;
        }

        public string GenerateQuickToken()
        {
            return _tokenService.GenerateToken("defaultuser");
        }
    }
}