using StudentsManagement.Client.Repository;
using StudentsManagement.Shared.Models;
using System.Net.Http.Json;

namespace StudentsManagement.Client.Services
{
    public class CountryService : ICountryRepository
    {
        private readonly HttpClient _httpClient;
        public CountryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Country> AddAsync(Country country)
        {
            var data = await _httpClient.PostAsJsonAsync("api/Countries/Add-Country", country);
            var respone = await data.Content.ReadFromJsonAsync<Country>();
            return respone;
        }

        public async Task<Country> DeleteAsync(int countryId)
        {
            var data = await _httpClient.PostAsJsonAsync("api/Countries/Delete-Country", countryId);
            var respone = await data.Content.ReadFromJsonAsync<Country>();
            return respone;
        }

        public async Task<Country> GetByIdAsync(int countryId)
        {
            var data = await _httpClient.GetAsync($"api/Countries/Single-Country/{countryId}");
            var respone = await data.Content.ReadFromJsonAsync<Country>();
            return respone;
        }

        public async Task<List<Country>> GetAllAsync()
        {
            var data = await _httpClient.GetAsync("api/Countries/All-Countries");
            var respone = await data.Content.ReadFromJsonAsync<List<Country>>();
            return respone;
        }

        public async Task<Country> UpdateAsync(Country country)
        {
            var data = await _httpClient.PostAsJsonAsync("api/Countries/Update-Country", country);
            var respone = await data.Content.ReadFromJsonAsync<Country>();
            return respone;
        }

        public async Task<PaginationModel<Country>> GetPagedCountriesAsync(int pageNumber, int pageSize)
        {
            var response = await _httpClient.GetFromJsonAsync<PaginationModel<Country>>(
                $"api/country?pageNumber={pageNumber}&pageSize={pageSize}");
            return response;
        }
    }
}
