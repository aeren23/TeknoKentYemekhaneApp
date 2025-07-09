using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace YemekhaneApp.Frontend.Security
{
    public class CustomAuthenticationHandler : AuthenticationHandler<CustomOptions>
    {
        private readonly CustomAuthenticationStateComponent auth;

        public CustomAuthenticationHandler(
            IOptionsMonitor<CustomOptions> options,
            ILoggerFactory logger, UrlEncoder encoder,
            CustomAuthenticationStateComponent auth)
            : base(options, logger, encoder)
        {
            this.auth = auth;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string token = Request.Cookies["auth_token"];
            if (string.IsNullOrEmpty(token))
                return AuthenticateResult.Fail("Authentication Failed");

            var userClaims = auth.VerifyUser(token);
            if (userClaims == null)
                return AuthenticateResult.Fail("Authentication Failed");

            var principal = new ClaimsPrincipal(new ClaimsIdentity(userClaims, "JWT"));
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Context.Response.Redirect("/login"); // Redirect to login page if authentication fails
            return Task.CompletedTask;
        }

        //protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        //{
        //    // Custom forbidden logic can be implemented here if needed
        //    return Task.CompletedTask;
        //}
    }



    public class CustomOptions : AuthenticationSchemeOptions
    {
        // Custom options can be added here if needed
    }
}
