using StudentsManagement.Client.Repository;
using StudentsManagement.Shared.Models;
using System.Net.Http.Json;

namespace StudentsManagement.Client.Services
{
    public class SystemCodeService : ISystemCodeRepository
    {
        private readonly HttpClient _httpClient;
        public SystemCodeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<SystemCode> AddAsync(SystemCode systemCode)
        {
            var data = await _httpClient.PostAsJsonAsync("api/SystemCodes/Add-SystemCode", systemCode);
            var respone = await data.Content.ReadFromJsonAsync<SystemCode>();
            return respone;
        }

        public async Task<SystemCode> DeleteAsync(int systemCodeId)
        {
            var data = await _httpClient.PostAsJsonAsync("api/SystemCodes/Delete-SystemCode", systemCodeId);
            var respone = await data.Content.ReadFromJsonAsync<SystemCode>();
            return respone;
        }

        public async Task<List<SystemCode>> GetAllAsync()
        {
            var data = await _httpClient.GetAsync("api/SystemCodes/All-SystemCodes");
            var respone = await data.Content.ReadFromJsonAsync<List<SystemCode>>();
            return respone;
        }

        public async Task<SystemCode> GetByIdAsync(int systemCodeId)
        {
            var data = await _httpClient.GetAsync($"api/SystemCodes/Single-SystemCode/{systemCodeId}");
            var respone = await data.Content.ReadFromJsonAsync<SystemCode>();
            return respone;
        }

        public async Task<SystemCode> UpdateAsync(SystemCode systemCode)
        {
            var data = await _httpClient.PostAsJsonAsync("api/SystemCodes/Update-SystemCode", systemCode);
            var respone = await data.Content.ReadFromJsonAsync<SystemCode>();
            return respone;
        }

        public async Task<PaginationModel<SystemCode>> GetPagedSystemCodesAsync(int pageNumber, int pageSize)
        {
            var response = await _httpClient.GetFromJsonAsync<PaginationModel<SystemCode>>(
                $"api/systemCode?pageNumber={pageNumber}&pageSize={pageSize}");
            return response;
        }
    }
}
