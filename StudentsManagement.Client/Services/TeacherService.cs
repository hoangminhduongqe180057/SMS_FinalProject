using Microsoft.JSInterop;
using StudentsManagement.Client.Repository;
using StudentsManagement.Shared.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace StudentsManagement.Client.Services
{
    public class TeacherService : ITeacherRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        public TeacherService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }
        public async Task<Teacher> AddAsync(Teacher teacher)
        {
            var data = await _httpClient.PostAsJsonAsync("api/Teachers/Add-Teacher", teacher);
            var respone = await data.Content.ReadFromJsonAsync<Teacher>();
            return respone;
        }

        public async Task<Teacher> DeleteAsync(int teacherId)
        {
            var data = await _httpClient.DeleteAsync($"api/Teachers/Delete-Teacher/{teacherId}");
            var respone = await data.Content.ReadFromJsonAsync<Teacher>();
            return respone;
        }

        public async Task<List<Teacher>> GetAllAsync()
        {
            var data = await _httpClient.GetAsync("api/Teachers/All-Teachers");
            var respone = await data.Content.ReadFromJsonAsync<List<Teacher>>();
            return respone;
        }

        public async Task<Teacher> GetByIdAsync(int teacherId)
        {
            var data = await _httpClient.GetAsync($"api/Teachers/Single-Teacher/{teacherId}");
            var respone = await data.Content.ReadFromJsonAsync<Teacher>();
            return respone;
        }

        public async Task<Teacher> UpdateAsync(Teacher teacher)
        {
            var data = await _httpClient.PostAsJsonAsync("api/Teachers/Update-Teacher", teacher);
            var respone = await data.Content.ReadFromJsonAsync<Teacher>();
            return respone;
        }

        public async Task<PaginationModel<Teacher>> GetPagedTeachersAsync(int pageNumber, int pageSize)
        {
            var response = await _httpClient.GetFromJsonAsync<PaginationModel<Teacher>>(
                $"api/teacher?pageNumber={pageNumber}&pageSize={pageSize}");
            return response;
        }

        public async Task<int> GetTotalTeachersCountAsync()
        {
            var totalTeachers = await _httpClient.GetFromJsonAsync<int>("api/Teachers/Total-Teachers");
            return totalTeachers;
        }

        public async Task<byte[]> ExportToCsvAsync()
        {
            var url = "/api/export/ExportTeachersToCsv";
            var response = await _httpClient.GetAsync(url);
            var csvContent = await response.Content.ReadAsByteArrayAsync();
            await _jsRuntime.InvokeVoidAsync("downloadFile", "Teachers.csv", "text/csv", csvContent);
            return new byte[0];
        }

        public async Task<PaginationModel<Teacher>> GetPagedTeachersAsync(int pageNumber, int pageSize, SearchParameters searchParameters = null)
        {
            var queryString = $"pageNumber={pageNumber}&pageSize={pageSize}";

            if (searchParameters != null)
            {
                if (!string.IsNullOrEmpty(searchParameters.Name))
                    queryString += $"&name={searchParameters.Name}";
                if (!string.IsNullOrEmpty(searchParameters.Email))
                    queryString += $"&email={searchParameters.Email}";
                if (!string.IsNullOrEmpty(searchParameters.PhoneNumber))
                    queryString += $"&phoneNumber={searchParameters.PhoneNumber}";

                // Thêm tham số sắp xếp
                queryString += $"&sortField={searchParameters.SortField}&sortAscending={searchParameters.SortAscending}";
            }

            var response = await _httpClient.GetFromJsonAsync<PaginationModel<Teacher>>($"api/teachers/paged?{queryString}");
            return response;
        }

        public async Task<bool> ImportTeachersAsync(Stream fileStream)
        {
            if (fileStream == null)
                return false;

            // Prepare MultipartFormDataContent with the provided Stream
            using var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(fileStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
            content.Add(streamContent, "file", "teachers_import.csv");

            // Send request to the backend
            var response = await _httpClient.PostAsync("api/Teacher/Import-Teachers", content);
            return response.IsSuccessStatusCode;
        }
    }
}
