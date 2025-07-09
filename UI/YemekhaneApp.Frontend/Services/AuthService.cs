using YemekhaneApp.Frontend.Models.Auth;

namespace YemekhaneApp.Frontend.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AuthResponseViewModel> IsTrustedUserAgent(string userAgent)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/istrusted", new { userAgent });

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AuthResponseViewModel>();
            }
            return null;
        }

        public async Task<AuthResponseViewModel> Authenticate(AuthRequestViewModel requestModel, string userAgent)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/auth/login")
            {
                Content = JsonContent.Create(requestModel)
            };
            request.Headers.Add("User-Agent", userAgent);

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AuthResponseViewModel>();
            }
            return null;
        }
    }
}
