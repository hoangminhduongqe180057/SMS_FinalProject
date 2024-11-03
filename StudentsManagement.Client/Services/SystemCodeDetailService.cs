using StudentsManagement.Client.Repository;
using StudentsManagement.Shared.Models;
using System.Net.Http.Json;

namespace StudentsManagement.Client.Services
{
    public class SystemCodeDetailService : ISystemCodeDetailRepository
    {
        private readonly HttpClient _httpClient;
        public SystemCodeDetailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<SystemCodeDetail> AddAsync(SystemCodeDetail systemCodeDetail)
        {
            var data = await _httpClient.PostAsJsonAsync("api/SystemCodeDetails/Add-SystemCodeDetail", systemCodeDetail);
            var respone = await data.Content.ReadFromJsonAsync<SystemCodeDetail>();
            return respone;
        }

        public async Task<SystemCodeDetail> DeleteAsync(int systemCodeDetailId)
        {
            var data = await _httpClient.PostAsJsonAsync("api/SystemCodeDetails/Delete-SystemCodeDetail", systemCodeDetailId);
            var respone = await data.Content.ReadFromJsonAsync<SystemCodeDetail>();
            return respone;
        }

        public async Task<List<SystemCodeDetail>> GetAllAsync()
        {
            var data = await _httpClient.GetAsync("api/SystemCodeDetails/All-SystemCodeDetails");
            var respone = await data.Content.ReadFromJsonAsync<List<SystemCodeDetail>>();
            return respone;
        }

        public async Task<SystemCodeDetail> GetByIdAsync(int systemCodeDetailId)
        {
            var data = await _httpClient.GetAsync($"api/SystemCodeDetails/Single-SystemCodeDetail/{systemCodeDetailId}");
            var respone = await data.Content.ReadFromJsonAsync<SystemCodeDetail>();
            return respone;
        }

        public async Task<SystemCodeDetail> UpdateAsync(SystemCodeDetail systemCodeDetail)
        {
            var data = await _httpClient.PostAsJsonAsync("api/SystemCodeDetails/Update-SystemCodeDetail", systemCodeDetail);
            var respone = await data.Content.ReadFromJsonAsync<SystemCodeDetail>();
            return respone;
        }

        public async Task<List<SystemCodeDetail>> GetByCodeAsync(string code)
        {
            var response = await _httpClient.GetFromJsonAsync<List<SystemCodeDetail>>($"api/SystemCodeDetails/ByCode/{code}");

            if (response == null)
            {
                return new List<SystemCodeDetail>();
            }

            return response;
        }

        public async Task<PaginationModel<SystemCodeDetail>> GetPagedSystemCodeDetailsAsync(int pageNumber, int pageSize)
        {
            var response = await _httpClient.GetFromJsonAsync<PaginationModel<SystemCodeDetail>>(
                $"api/systemCodeDetail?pageNumber={pageNumber}&pageSize={pageSize}");
            return response;
        }

        public async Task<SystemCodeDetail> GetByStatusCodeAsync(string code, string statusCode)
        {
            var response = await _httpClient.GetFromJsonAsync<List<SystemCodeDetail>>($"api/SystemCodeDetails/ByStatusCode/{code}");
            if (response == null)
            {
                return new SystemCodeDetail(); 
            }
            return response?.FirstOrDefault(); ;
        }
    }
}
