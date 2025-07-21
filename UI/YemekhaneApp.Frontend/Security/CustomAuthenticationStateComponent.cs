using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using YemekhaneApp.Frontend.Helpers;
using YemekhaneApp.Frontend.Models.Auth;
using YemekhaneApp.Frontend.Services;

namespace YemekhaneApp.Frontend.Security
{
    public class CustomAuthenticationStateComponent
    {
        private readonly CookieHelper _cookieHelper;
        private readonly AuthService _authService;
        private readonly UserAgentHelper _userAgentHelper;

        private readonly string _cookieKey = "auth_token";
        private readonly string key = "1asfhDFAwRTbsTGDSkdQakjlwehqw-123!!-asAdf-q451wsf6";

        public CustomAuthenticationStateComponent(
            CookieHelper cookieHelper,
            AuthService authService,
            UserAgentHelper userAgentHelper)
        {
            _cookieHelper = cookieHelper;
            _authService = authService;
            _userAgentHelper = userAgentHelper;
        }

        /// <summary>
        /// Şifre ile login olur, token döner ve cookie'ye yazar
        /// </summary>
        public async Task<string> AuthenticateAsync(AuthRequestViewModel loginRequest)
        {
            var userAgent = await _userAgentHelper.GetUserAgentAsync();
            var response = await _authService.Authenticate(loginRequest, userAgent);

            if (response != null && !string.IsNullOrEmpty(response.Token))
            {
                await _cookieHelper.SetCookieAsync(_cookieKey, response.Token, 1);
                return response.Token;
            }

            return null;
        }

        /// <summary>
        /// Trusted device kontrolü yapar, varsa token'ı cookie'ye yazar
        /// </summary>
        public async Task<string> CheckTrustedAsync()
        {
            var userAgent = await _userAgentHelper.GetUserAgentAsync();
            var response = await _authService.IsTrustedUserAgent(userAgent);

            if (response != null && !string.IsNullOrEmpty(response.Token))
            {
                await _cookieHelper.SetCookieAsync(_cookieKey, response.Token, 1);
                return response.Token;
            }

            return null;
        }

        /// <summary>
        /// Cookie'deki token'ı siler
        /// </summary>
        public async Task DeleteTokenAsync()
        {
            await _cookieHelper.DeleteCookieAsync(_cookieKey);
        }

        /// <summary>
        /// Cookie'deki token'ı alır
        /// </summary>
        public async Task<string> GetTokenAsync()
        {
            return await _cookieHelper.GetCookieAsync(_cookieKey);
        }

        /// <summary>
        /// Verify the user by validating the JWT token and returning claims if valid
        /// </summary>
        public IEnumerable<Claim>? VerifyUser(string token)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true // Token süresi kontrolü açık kalsın, güvenlik için önemli
            };

            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                var jsonToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
                if (jsonToken != null)
                {
                    return jsonToken.Claims.ToList();
                }
            }
            catch (Exception)
            {
            }

            return null;
        }
    }
}
