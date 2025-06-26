using System.Net.Http.Json;
using YemekhaneApp.Frontend.Models.Employee;
using YemekhaneApp.Frontend.Models.UserDebt;

namespace YemekhaneApp.Frontend.Services
{
    public class UserDebtService
    {
        private readonly HttpClient _httpClient;

        public UserDebtService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UserDebtViewModel>> GetAllUserDebtsAsync()
        {
            var response = await _httpClient.GetAsync("api/UserDebt");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return new List<UserDebtViewModel>();

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<List<UserDebtViewModel>>();
            return result ?? new List<UserDebtViewModel>();
        }

        public async Task<List<UserDebtViewModel>> GetUserDebtsByEmployeeIdAsync(Guid employeeId)
        {
            var response = await _httpClient.GetAsync($"api/UserDebt/employee/{employeeId}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return new List<UserDebtViewModel>();

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<List<UserDebtViewModel>>();
            return result ?? new List<UserDebtViewModel>();
        }

        public async Task<UserDebtViewModel?> GetUserDebtByEmployeeIdAndMonthAsync(Guid employeeId, int year, int month)
        {
            var response = await _httpClient.GetAsync($"api/UserDebt/employee/{employeeId}/year/{year}/month/{month}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UserDebtViewModel>();
        }

        public async Task<List<EmployeeViewModel>> GetEmployeesWithUserDebtAsync()
        {
            var response = await _httpClient.GetAsync("api/UserDebt/with-debt");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return new List<EmployeeViewModel>();

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<List<EmployeeViewModel>>();
            return result ?? new List<EmployeeViewModel>();
        }

        public async Task UpdateUserDebtAsync(UserDebtViewModel userDebt)
        {
            // userDebt.Id, userDebt.Amount ve userDebt.IsPaid alanları dolu olmalı
            var response = await _httpClient.PutAsJsonAsync($"api/UserDebt/{userDebt.Id}", userDebt);
            response.EnsureSuccessStatusCode();
        }

        public async Task<Guid> CreateUserDebtAsync(UserDebtCreateViewModel userDebt)
        {
            var response = await _httpClient.PostAsJsonAsync("api/UserDebt", userDebt);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Guid>();
        }

        public async Task DeleteUserDebtAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/UserDebt/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}