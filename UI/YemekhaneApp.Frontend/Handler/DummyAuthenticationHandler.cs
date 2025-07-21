using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;


namespace YemekhaneApp.Frontend.Handler
{

    // Sadece [Authorize] attribute'unun çalışması için dummy handler
    public class DummyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public DummyAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder)
            : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Gerçek authentication CustomAuthenticationStateProvider'da yapılıyor
            return Task.FromResult(AuthenticateResult.NoResult());
        }
    }

}
