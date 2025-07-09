using Microsoft.JSInterop;

namespace YemekhaneApp.Frontend.Helpers
{
    public class UserAgentHelper
    {
        private readonly IJSRuntime _jsRuntime;

        public UserAgentHelper(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<string> GetUserAgentAsync()
        {
            return await _jsRuntime.InvokeAsync<string>("getUserAgent");
        }
    }
}
