using Microsoft.JSInterop;
using YemekhaneApp.Frontend.Models.Auth;
using static System.Net.WebRequestMethods;

namespace YemekhaneApp.Frontend.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;


        public AuthService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
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
                return result;
            }
            return null;
        }
    }
}
