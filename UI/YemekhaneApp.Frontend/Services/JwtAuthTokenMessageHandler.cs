using System.Net.Http.Headers;
using Blazored.SessionStorage;

namespace YemekhaneApp.Frontend.Services
{
    public class JwtAuthTokenMessageHandler : DelegatingHandler
    {
        private readonly ISessionStorageService _sessionStorageService;

        public JwtAuthTokenMessageHandler(ISessionStorageService sessionStorageService)
        {
            _sessionStorageService = sessionStorageService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _sessionStorageService.GetItemAsync<string>("token");
            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}