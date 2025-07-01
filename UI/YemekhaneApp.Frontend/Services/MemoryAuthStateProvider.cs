using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.JSInterop;

namespace YemekhaneApp.Frontend.Services
{
    public class MemoryAuthStateProvider : AuthenticationStateProvider
    {
        private readonly AuthStateService _authStateService;

        public MemoryAuthStateProvider(AuthStateService authStateService)
        {
            _authStateService = authStateService;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = _authStateService.IsAuthenticated
                ? new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "User") }, "memory")
                : new ClaimsIdentity();

            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
        }

        public void NotifyAuthStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}