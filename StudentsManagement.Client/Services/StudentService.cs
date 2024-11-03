using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using StudentsManagement.Shared.Models;
using StudentsManagement.Shared.StudentRepository;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace StudentsManagement.Client.Services
{
    public class StudentService : IStudentRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        public StudentService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }
        public async Task<Student> AddStudentAsync(Student student)
        {
            var newstudent = await _httpClient.PostAsJsonAsync("api/Student/Add-Student", student);
            var respone = await newstudent.Content.ReadFromJsonAsync<Student>();
            return respone;
        }

        public async Task<Student> DeleteStudentAsync(int studentId)
        {
            var deletestudent = await _httpClient.DeleteAsync($"api/Student/Delete-Student/{studentId}");
            if (deletestudent.IsSuccessStatusCode)
            {
                var respone = await deletestudent.Content.ReadFromJsonAsync<Student>();
                return respone;
            }
            return null;
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            var allstudents = await _httpClient.GetAsync("api/Student/All-Students");
            var respone = await allstudents.Content.ReadFromJsonAsync<List<Student>>();
            return respone;
        }

        public async Task<Student> GetStudentByIdAsync(int studentId)
        {
            var singlestudent = await _httpClient.GetAsync($"api/Student/Single-Student/{studentId}");
            if (singlestudent.IsSuccessStatusCode)
            {
                var respone = await singlestudent.Content.ReadFromJsonAsync<Student>();
                return respone;
            }
            return null;
        }

        public async Task<Student> UpdateStudentAsync(Student student)
        {
            var newstudent = await _httpClient.PostAsJsonAsync("api/Student/Update-Student", student);
            var respone = await newstudent.Content.ReadFromJsonAsync<Student>();
            return respone;
        }

        public async Task<PaginationModel<Student>> GetPagedStudentsAsync(int pageNumber, int pageSize)
        {
            var response = await _httpClient.GetFromJsonAsync<PaginationModel<Student>>(
                $"api/students?pageNumber={pageNumber}&pageSize={pageSize}");
            return response;
        }

        public async Task<int> GetTotalStudentsCountAsync()
        {
            var totalStudents = await _httpClient.GetFromJsonAsync<int>("api/Student/Total-Students");
            return totalStudents;
        }

        public async Task<byte[]> ExportToCsvAsync()
        {
            var url = "/api/export/ExportStudentsToCsv";
            var response = await _httpClient.GetAsync(url);
            var csvContent = await response.Content.ReadAsByteArrayAsync();
            await _jsRuntime.InvokeVoidAsync("downloadFile", "Students.csv", "text/csv", csvContent);
            return new byte[0];
        }

        public async Task<PaginationModel<Student>> GetPagedStudentsAsync(int pageNumber, int pageSize, SearchParameters searchParameters = null)
        {
            var queryString = $"pageNumber={pageNumber}&pageSize={pageSize}";

            if (searchParameters != null)
            {
                if (!string.IsNullOrEmpty(searchParameters.RegistrationNo))
                    queryString += $"&registrationNo={searchParameters.RegistrationNo}";
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

            var response = await _httpClient.GetFromJsonAsync<PaginationModel<Student>>($"api/students/paged?{queryString}");
            return response;
        }



        public async Task<bool> ImportStudentsAsync(Stream fileStream)
        {
            if (fileStream == null)
                return false;

            // Prepare MultipartFormDataContent with the provided Stream
            using var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(fileStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
            content.Add(streamContent, "file", "students_import.csv");

            // Send request to the backend
            var response = await _httpClient.PostAsync("api/Student/Import-Students", content);
            return response.IsSuccessStatusCode;
        }
    }
}
