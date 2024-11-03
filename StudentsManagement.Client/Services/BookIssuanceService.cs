using StudentsManagement.Client.Repository;
using StudentsManagement.Shared.Models;
using System.Net;
using System.Net.Http.Json;

namespace StudentsManagement.Client.Services
{
    public class BookIssuanceService : IBookIssuanceRepository
    {
        private readonly HttpClient _httpClient;
        public BookIssuanceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<BookIssuance> AddAsync(BookIssuance bookIssuance)
        {
            var data = await _httpClient.PostAsJsonAsync("api/BookIssuances/Add-BookIssuance", bookIssuance);
            var respone = await data.Content.ReadFromJsonAsync<BookIssuance>();
            return respone;
        }

        public async Task<BookIssuance> DeleteAsync(int bookIssuanceId)
        {
            var data = await _httpClient.PostAsJsonAsync("api/BookIssuances/Delete-BookIssuance", bookIssuanceId);
            var respone = await data.Content.ReadFromJsonAsync<BookIssuance>();
            return respone;
        }

        public async Task<List<BookIssuance>> GetAllAsync()
        {
            var data = await _httpClient.GetAsync("api/BookIssuances/All-BookIssuances");
            var respone = await data.Content.ReadFromJsonAsync<List<BookIssuance>>();
            return respone;
        }

        public async Task<BookIssuance> GetByIdAsync(int bookIssuanceId)
        {
            var data = await _httpClient.GetAsync($"api/BookIssuances/Single-BookIssuance/{bookIssuanceId}");
            var respone = await data.Content.ReadFromJsonAsync<BookIssuance>();
            return respone;
        }

        public async Task<BookIssuance> ReturnBookUpdateAsync(BookIssuance bookIssuance)
        {
            var data = await _httpClient.PostAsJsonAsync("api/BookIssuances/ReturnUpdate-BookIssuance", bookIssuance);
            var respone = await data.Content.ReadFromJsonAsync<BookIssuance>();
            return respone;
        }

        public async Task<BookIssuance> UpdateAsync(BookIssuance bookIssuance)
        {
            var data = await _httpClient.PostAsJsonAsync("api/BookIssuances/Update-BookIssuance", bookIssuance);
            var respone = await data.Content.ReadFromJsonAsync<BookIssuance>();
            return respone;
        }

        public async Task<PaginationModel<BookIssuance>> GetPagedBookIssuancesAsync(int pageNumber, int pageSize)
        {
            var response = await _httpClient.GetFromJsonAsync<PaginationModel<BookIssuance>>(
                $"api/BbookIssuance?pageNumber={pageNumber}&pageSize={pageSize}");
            return response;
        }
        public async Task<PaginationModel<BookIssuance>> GetPagedBookReturnedAsync(int pageNumber, int pageSize)
        {
            var response = await _httpClient.GetFromJsonAsync<PaginationModel<BookIssuance>>(
                $"api/BbookReturned?pageNumber={pageNumber}&pageSize={pageSize}");
            return response;
        }
    }
}
