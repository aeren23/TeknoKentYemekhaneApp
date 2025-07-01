using Blazored.SessionStorage;
using Microsoft.JSInterop;
using YemekhaneApp.Frontend.Models.Auth;

namespace YemekhaneApp.Frontend.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        private readonly ISessionStorageService _sessionStorageService;

        public AuthService(HttpClient httpClient, IJSRuntime jsRuntime, ISessionStorageService sessionStorageService)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
            _sessionStorageService = sessionStorageService;
        }

        public async Task<AuthResponseViewModel> IsTrustedUserAgent(string userAgent)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/istrusted", new { userAgent });

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AuthResponseViewModel>();
                return result;
            }
            return null;
        }

        public async Task<AuthResponseViewModel> Authenticate(AuthRequestViewModel password)
        {
            var userAgent = await _jsRuntime.InvokeAsync<string>("getUserAgent");
            var request = new HttpRequestMessage(HttpMethod.Post, "api/auth/login")
            {
                Content = JsonContent.Create(password)
            };
            request.Headers.Add("User-Agent", userAgent);

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AuthResponseViewModel>();
                if (result != null && !string.IsNullOrEmpty(result.Token))
                {
                    await _sessionStorageService.SetItemAsync("token", result.Token);
                }
                return result;
            }
            return null;
        }

        public async Task LogoutAsync()
        {
            await _sessionStorageService.RemoveItemAsync("token");
        }

        public async Task<string> GetTokenFromSessionAsync()
        {
            return await _sessionStorageService.GetItemAsync<string>("token");
        }

        public async Task SetTokenToSessionAsync(string token)
        {
            await _sessionStorageService.SetItemAsync("token", token);
        }
    }
}