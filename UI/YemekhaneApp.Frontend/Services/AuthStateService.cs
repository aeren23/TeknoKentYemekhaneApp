namespace YemekhaneApp.Frontend.Services
{
    public class AuthStateService
    {
        public string JwtToken { get; private set; }
        public bool IsAuthenticated => !string.IsNullOrEmpty(JwtToken);

        public void SetToken(string token)
        {
            JwtToken = token;
        }

        public void ClearToken()
        {
            JwtToken = null;
        }
    }
}
