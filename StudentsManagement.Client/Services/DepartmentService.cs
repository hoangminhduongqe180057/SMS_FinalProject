using Microsoft.JSInterop;
using StudentsManagement.Client.Repository;
using StudentsManagement.Shared.Models;
using System.Net.Http.Json;

namespace StudentsManagement.Client.Services
{
    public class DepartmentService : IDepartmentRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        public DepartmentService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }
        public async Task<Department> AddAsync(Department department)
        {
            var data = await _httpClient.PostAsJsonAsync("api/Departments/Add-Department", department);
            var respone = await data.Content.ReadFromJsonAsync<Department>();
            return respone;
        }

        public async Task<Department> DeleteAsync(int departmentId)
        {
            var response = await _httpClient.DeleteAsync($"api/Departments/Delete-Department/{departmentId}");

            if (response.IsSuccessStatusCode)
            {
                // Ensure we're attempting to read a JSON object only if data is returned
                return await response.Content.ReadFromJsonAsync<Department>();
            }

            return null;
        }


        public async Task<List<Department>> GetAllAsync()
        {
            var data = await _httpClient.GetAsync("api/Departments/All-Departments");
            var respone = await data.Content.ReadFromJsonAsync<List<Department>>();
            return respone;
        }

        public async Task<Department> GetByIdAsync(int departmentId)
        {
            var data = await _httpClient.GetAsync($"api/Departments/Single-Department/{departmentId}");
            var respone = await data.Content.ReadFromJsonAsync<Department>();
            return respone;
        }

        public async Task<Department> UpdateAsync(Department department)
        {
            var data = await _httpClient.PostAsJsonAsync("api/Departments/Update-Department", department);
            var respone = await data.Content.ReadFromJsonAsync<Department>();
            return respone;
        }

        public async Task<PaginationModel<Department>> GetPagedDepartmentsAsync(int pageNumber, int pageSize)
        {
            var response = await _httpClient.GetFromJsonAsync<PaginationModel<Department>>(
                $"api/department?pageNumber={pageNumber}&pageSize={pageSize}");
            return response;
        }

        public async Task<byte[]> ExportToCsvAsync()
        {
            var url = "/api/Departments/ExportDepartmentsToCsv";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                // Lấy nội dung của CSV dưới dạng mảng byte
                var csvContent = await response.Content.ReadAsByteArrayAsync();
                // Gọi hàm JavaScript để tải xuống tệp
                await _jsRuntime.InvokeVoidAsync("downloadFile", "Departments.csv", "text/csv", csvContent);
                return csvContent;
            }
            else
            {
                // Trả về mảng byte rỗng nếu yêu cầu không thành công
                return new byte[0];
            }
        }


        public async Task<PaginationModel<Department>> GetPagedDepartmentsAsync(int pageNumber, int pageSize, SearchParameters searchParameters = null)
        {
            var queryString = $"pageNumber={pageNumber}&pageSize={pageSize}";

            if (searchParameters != null)
            {
                if (!string.IsNullOrEmpty(searchParameters.Name))
                    queryString += $"&name={searchParameters.Name}";

                if (!string.IsNullOrEmpty(searchParameters.Code))
                    queryString += $"&code={searchParameters.Code}";

                // Thêm tham số sắp xếp
                queryString += $"&sortField={searchParameters.SortField}&sortAscending={searchParameters.SortAscending}";
            }

            var response = await _httpClient.GetFromJsonAsync<PaginationModel<Department>>($"api/departments/paged?{queryString}");
            return response;
        }
    }
}
