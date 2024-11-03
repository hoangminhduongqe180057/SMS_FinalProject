using Microsoft.JSInterop;
using StudentsManagement.Client.Repository;
using StudentsManagement.Shared.Models;
using System.Net.Http.Json;

namespace StudentsManagement.Client.Services
{
    public class ParentService : IParentRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        public ParentService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }
        public async Task<Parent> AddAsync(Parent parent)
        {
            var newparent = await _httpClient.PostAsJsonAsync("api/Parent/Add-Parent", parent);
            var respone = await newparent.Content.ReadFromJsonAsync<Parent>();
            return respone;
        }

        public async Task<Parent> DeleteAsync(int parentId)
        {
            var deleteparent = await _httpClient.DeleteAsync($"api/Parent/Delete-Parent/{parentId}");
            var respone = await deleteparent.Content.ReadFromJsonAsync<Parent>();
            return respone;
        }

        public async Task<List<Parent>> GetAllAsync()
        {
            var allparents = await _httpClient.GetAsync("api/Parent/All-Parents");
            var respone = await allparents.Content.ReadFromJsonAsync<List<Parent>>();
            return respone;
        }

        public async Task<Parent> GetByIdAsync(int parentId)
        {
            var singleparent = await _httpClient.GetAsync($"api/Parent/Single-Parent/{parentId}");
            var respone = await singleparent.Content.ReadFromJsonAsync<Parent>();
            return respone;
        }

        public async Task<Parent> UpdateAsync(Parent parent)
        {
            var newparent = await _httpClient.PostAsJsonAsync("api/Parent/Update-Parent", parent);
            var respone = await newparent.Content.ReadFromJsonAsync<Parent>();
            return respone;
        }

        public async Task<PaginationModel<Parent>> GetPagedParentsAsync(int pageNumber, int pageSize)
        {
            var response = await _httpClient.GetFromJsonAsync<PaginationModel<Parent>>(
                $"api/parent?pageNumber={pageNumber}&pageSize={pageSize}");
            return response;
        }

        public async Task<byte[]> ExportToCsvAsync()
        {
            var url = "/api/export/ExportParentsToCsv";
            var response = await _httpClient.GetAsync(url);
            var csvContent = await response.Content.ReadAsByteArrayAsync();
            await _jsRuntime.InvokeVoidAsync("downloadFile", "Parents.csv", "text/csv", csvContent);
            return new byte[0];
        }

        public async Task<PaginationModel<Parent>> GetPagedParentsAsync(int pageNumber, int pageSize, SearchParameters searchParameters = null)
        {
            var queryString = $"pageNumber={pageNumber}&pageSize={pageSize}";

            if (searchParameters != null)
            {
                if (!string.IsNullOrEmpty(searchParameters.StudentName))
                    queryString += $"&registrationNo={searchParameters.StudentName}";
                if (!string.IsNullOrEmpty(searchParameters.Name))
                    queryString += $"&name={searchParameters.Name}";
                if (!string.IsNullOrEmpty(searchParameters.Email))
                    queryString += $"&email={searchParameters.Email}";
                if (!string.IsNullOrEmpty(searchParameters.PhoneNumber))
                    queryString += $"&phoneNumber={searchParameters.PhoneNumber}";
                if (!string.IsNullOrEmpty(searchParameters.Class))
                    queryString += $"&class={searchParameters.Class}";

                // Thêm tham số sắp xếp
                queryString += $"&sortField={searchParameters.SortField}&sortAscending={searchParameters.SortAscending}";
            }

            var response = await _httpClient.GetFromJsonAsync<PaginationModel<Parent>>($"api/parents/paged?{queryString}");
            return response;
        }
    }
}
