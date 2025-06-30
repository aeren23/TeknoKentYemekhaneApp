using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YemekhaneApp.Application.DTOs.Auth;

namespace YemekhaneApp.Application.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthResponseDto> AuthenticateAsync(string password, string userAgent);
        Task<bool> IsTrustedDeviceAsync(string userAgent);
        public string GenerateQuickToken();
    }
}
