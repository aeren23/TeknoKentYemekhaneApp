using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using YemekhaneApp.Frontend.Models.Auth;

namespace YemekhaneApp.Frontend.Security
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly CustomAuthenticationStateComponent customAuthenticationStateComponent;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());


        public CustomAuthenticationStateProvider(CustomAuthenticationStateComponent customAuthenticationStateComponent)
        {
            this.customAuthenticationStateComponent = customAuthenticationStateComponent;
        }


        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await customAuthenticationStateComponent.GetTokenAsync(); // cookie’den token çek

                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();
                    var readJWT = handler.ReadJwtToken(token);

                    var identity = new ClaimsIdentity(readJWT.Claims, "JWT");
                    var user = new ClaimsPrincipal(identity);
                    return new AuthenticationState(user);
                }
                else
                {
                    // Kimlik yok => anonim kullanıcı
                    var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
                    return new AuthenticationState(anonymous);
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda anonim kullanıcı döndür
                Console.WriteLine($"Error in GetAuthenticationStateAsync: {ex.Message}");
                return new AuthenticationState(_anonymous);
            }
        }

        public async Task LogoutAsync()
        {
            await customAuthenticationStateComponent.DeleteTokenAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }


        public async Task<bool> AuthenticateUser(AuthRequestViewModel loginRequest)
        {
            var token = await customAuthenticationStateComponent.AuthenticateAsync(loginRequest);

            if (!string.IsNullOrEmpty(token))
            {
                var readJWT = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var identity = new ClaimsIdentity(readJWT.Claims, "JWT");
                var user = new ClaimsPrincipal(identity);
                var state = new AuthenticationState(user);

                NotifyAuthenticationStateChanged(Task.FromResult(state));
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> AuthenticateTrustedUserAsync()
        {
            var token = await customAuthenticationStateComponent.CheckTrustedAsync();

            if (!string.IsNullOrEmpty(token))
            {
                var readJWT = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var identity = new ClaimsIdentity(readJWT.Claims, "JWT");
                var user = new ClaimsPrincipal(identity);
                var state = new AuthenticationState(user);

                NotifyAuthenticationStateChanged(Task.FromResult(state));
                return true;
            }

            return false;
        }
    }
}
