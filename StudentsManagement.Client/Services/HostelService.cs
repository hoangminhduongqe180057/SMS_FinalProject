using StudentsManagement.Client.Repository;
using StudentsManagement.Shared.Models;
using System.Net.Http.Json;

namespace StudentsManagement.Client.Services
{
    public class HostelService : IHostelRepository
    {
        private readonly HttpClient _httpClient;
        public HostelService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Hostel> AddAsync(Hostel Hostel)
        {
            var data = await _httpClient.PostAsJsonAsync("api/Hostels/Add-Hostel", Hostel);
            var respone = await data.Content.ReadFromJsonAsync<Hostel>();
            return respone;
        }

        public async Task<Hostel> DeleteAsync(int HostelId)
        {
            var data = await _httpClient.PostAsJsonAsync("api/Hostels/Delete-Hostel", HostelId);
            var respone = await data.Content.ReadFromJsonAsync<Hostel>();
            return respone;
        }

        public async Task<List<Hostel>> GetAllAsync()
        {
            var data = await _httpClient.GetAsync("api/Hostels/All-Hostels");
            var respone = await data.Content.ReadFromJsonAsync<List<Hostel>>();
            return respone;
        }

        public async Task<Hostel> GetByIdAsync(int HostelId)
        {
            var data = await _httpClient.GetAsync($"api/Hostels/Single-Hostel/{HostelId}");
            var respone = await data.Content.ReadFromJsonAsync<Hostel>();
            return respone;
        }

        public async Task<PaginationModel<Hostel>> GetPagedHostelsAsync(int pageNumber, int pageSize)
        {
            var response = await _httpClient.GetFromJsonAsync<PaginationModel<Hostel>>(
                $"api/Hostels?pageNumber={pageNumber}&pageSize={pageSize}");
            return response;
        }

        public async Task<Hostel> UpdateAsync(Hostel Hostel)
        {
            var data = await _httpClient.PostAsJsonAsync("api/Hostels/ReturnUpdate-Hostels", Hostel);
            var respone = await data.Content.ReadFromJsonAsync<Hostel>();
            return respone;
        }

        public async Task<int> GetTotalHostelsCountAsync()
        {
            var totalHostels = await _httpClient.GetFromJsonAsync<int>("api/Hostels/Total-Hostels");
            return totalHostels;
        }
    }
}
