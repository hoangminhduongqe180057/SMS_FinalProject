using Microsoft.JSInterop;
using StudentsManagement.Client.Repository;
using StudentsManagement.Shared.Models;
using System.Net.Http.Json;

namespace StudentsManagement.Client.Services

{
    public class SubjectService : ISubjectRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        public SubjectService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }
        public async Task<Subject> AddAsync(Subject subject)
        {
            var data = await _httpClient.PostAsJsonAsync("api/Subjects/Add-Subject", subject);
            var respone = await data.Content.ReadFromJsonAsync<Subject>();
            return respone;
        }

        public async Task<Subject> DeleteAsync(int subjectId)
        {
            var data = await _httpClient.PostAsJsonAsync("api/Subjects/Delete-Subject", subjectId);
            var respone = await data.Content.ReadFromJsonAsync<Subject>();
            return respone;
        }

        public async Task<List<Subject>> GetAllAsync()
        {
            var data = await _httpClient.GetAsync("api/Subjects/All-Subjects");
            var respone = await data.Content.ReadFromJsonAsync<List<Subject>>();
            return respone;
        }

        public async Task<Subject> GetByIdAsync(int subjectId)
        {
            var data = await _httpClient.GetAsync($"api/Subjects/Single-Subject/{subjectId}");
            var respone = await data.Content.ReadFromJsonAsync<Subject>();
            return respone;
        }

        public async Task<Subject> UpdateAsync(Subject subject)
        {
            var data = await _httpClient.PostAsJsonAsync("api/Subjects/Update-Subject", subject);
            var respone = await data.Content.ReadFromJsonAsync<Subject>();
            return respone;
        }

        public async Task<PaginationModel<Subject>> GetPagedSubjectsAsync(int pageNumber, int pageSize)
        {
            var response = await _httpClient.GetFromJsonAsync<PaginationModel<Subject>>(
                $"api/subject?pageNumber={pageNumber}&pageSize={pageSize}");
            return response;
        }

        public async Task<byte[]> ExportToCsvAsync()
        {
            var url = "/api/export/ExportSubjectsToCsv";
            var response = await _httpClient.GetAsync(url);
            var csvContent = await response.Content.ReadAsByteArrayAsync();
            await _jsRuntime.InvokeVoidAsync("downloadFile", "Subjects.csv", "text/csv", csvContent);
            return new byte[0];
        }

        public async Task<PaginationModel<Subject>> GetPagedSubjectsAsync(int pageNumber, int pageSize, SearchParameters searchParameters = null)
        {
            var queryString = $"pageNumber={pageNumber}&pageSize={pageSize}";

            if (searchParameters != null)
            {
                if (!string.IsNullOrEmpty(searchParameters.Name))
                    queryString += $"&name={searchParameters.Name}";

                // Thêm tham số sắp xếp
                queryString += $"&sortField={searchParameters.SortField}&sortAscending={searchParameters.SortAscending}";
            }

            var response = await _httpClient.GetFromJsonAsync<PaginationModel<Subject>>($"api/subjects/paged?{queryString}");
            return response;
        }
    }
}
