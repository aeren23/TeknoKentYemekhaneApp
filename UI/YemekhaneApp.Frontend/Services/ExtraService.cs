using System.Net.Http.Json;
using YemekhaneApp.Frontend.Models.Extra;

namespace YemekhaneApp.Frontend.Services
{
    public class ExtraService
    {
        private readonly HttpClient _httpClient;

        public ExtraService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ExtraViewModel>> GetAllExtrasAsync()
        {
            var response = await _httpClient.GetAsync("api/Extra");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return new List<ExtraViewModel>();

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<List<ExtraViewModel>>();
            return result ?? new List<ExtraViewModel>();
        }

        public async Task<ExtraViewModel?> GetExtraByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/Extra/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ExtraViewModel>();
        }

        public async Task<Guid?> CreateExtraAsync(ExtraCreateViewModel extra)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Extra", extra);
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<Guid>();
        }

        public async Task<bool> UpdateExtraAsync(ExtraUpdateViewModel extra)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Extra/{extra.Id}", extra);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteExtraAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/Extra/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
