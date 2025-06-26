using YemekhaneApp.Frontend.Models.MealRecord;
using YemekhaneApp.Frontend.Models.Extra;

namespace YemekhaneApp.Frontend.Services
{
    public class MealRecordService
    {
        private readonly HttpClient _httpClient;
        public MealRecordService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<DateOnly>> GetMealDatesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<DateOnly>>("api/mealrecord/dates");
        }

        public async Task<List<MealRecordViewModel>> GetMealRecordsAsync()
        {
            var response = await _httpClient.GetAsync("api/mealrecord/get-all");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return new List<MealRecordViewModel>();

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(content))
                return new List<MealRecordViewModel>();

            return await _httpClient.GetFromJsonAsync<List<MealRecordViewModel>>("api/mealrecord/get-all") ?? new List<MealRecordViewModel>();
        }

        public async Task<List<MealRecordViewModel>> GetMealRecordsByEmployeeIdAsync(Guid employeeId)
        {
            var response = await _httpClient.GetAsync($"api/mealrecord/employee/{employeeId}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return new List<MealRecordViewModel>();

            response.EnsureSuccessStatusCode();

            return await _httpClient.GetFromJsonAsync<List<MealRecordViewModel>>($"api/mealrecord/employee/{employeeId}") ?? new List<MealRecordViewModel>();
        }

        public async Task<MealRecordViewModel> GetMealRecordByIdAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<MealRecordViewModel>($"api/mealrecord/{id}");
        }

        public async Task<List<MealRecordViewModel>> GetMealRecordsByDateAsync(DateOnly date)
        {
            return await _httpClient.GetFromJsonAsync<List<MealRecordViewModel>>($"api/mealrecord/employee/{date}");
        }

        public async Task<List<MealRecordWithEmployeeViewModel>> GetMealRecordsByDateWithEmployeesAsync(Guid id)
        {
            // API endpoint: api/mealrecord/by-date/{date}
            // DateOnly için yyyy-MM-dd formatı kullanılır
            var response = await _httpClient.GetAsync($"api/mealrecord/by-date/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<List<MealRecordWithEmployeeViewModel>>();
            return result ?? new List<MealRecordWithEmployeeViewModel>();
        }

        public async Task AddMealRecordAsync(MealRecordCreateViewModel mealRecord)
        {
            var Year = mealRecord.MealDate.Year;
            var Month = mealRecord.MealDate.Month;
            var Day = mealRecord.MealDate.Day;
            var now = DateTime.Now;
            if (Year == now.Year && Month == now.Month && Day <= now.Day)
            {
                // ExtraIds zaten MealRecordCreateViewModel'de var
                var response = await _httpClient.PostAsJsonAsync("api/mealrecord", mealRecord);
                response.EnsureSuccessStatusCode();
            }
            else
            {
                throw new InvalidOperationException("You can only add meal records for today or earlier dates.");
            }
        }

        public async Task UpdateMealRecordAsync(MealRecordUpdateViewModel mealRecord)
        {
            // ExtraIds zaten MealRecordUpdateViewModel'de var
            var response = await _httpClient.PutAsJsonAsync("api/mealrecord", mealRecord);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteMealRecordAsync(Guid id)
        {
            await _httpClient.DeleteAsync($"api/mealrecord/{id}");
        }
    }
}
