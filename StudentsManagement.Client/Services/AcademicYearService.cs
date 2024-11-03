using StudentsManagement.Client.Repository;
using StudentsManagement.Shared.Models;
using System.Net.Http.Json;

namespace StudentsManagement.Client.Services
{
    public class AcademicYearService : IAcademicYearRepository
    {
        private readonly HttpClient _httpClient;
        public AcademicYearService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AcademicYears> AddAsync(AcademicYears academicYears)
        {
            var data = await _httpClient.PostAsJsonAsync("api/AcademicYears/Add-AcademicYear", academicYears);
            var respone = await data.Content.ReadFromJsonAsync<AcademicYears>();
            return respone;
        }

        public async Task<AcademicYears> DeleteAsync(int academicYearsId)
        {
            var data = await _httpClient.PostAsJsonAsync("api/AcademicYears/Delete-AcademicYear", academicYearsId);
            var respone = await data.Content.ReadFromJsonAsync<AcademicYears>();
            return respone;
        }

        public async Task<List<AcademicYears>> GetAllAsync()
        {
            var data = await _httpClient.GetAsync("api/AcademicYears/All-AcademicYears");
            var respone = await data.Content.ReadFromJsonAsync<List<AcademicYears>>();
            return respone;
        }

        public async Task<AcademicYears> GetByIdAsync(int academicYearsId)
        {
            var data = await _httpClient.GetAsync($"api/AcademicYears/Single-AcademicYear/{academicYearsId}");
            var respone = await data.Content.ReadFromJsonAsync<AcademicYears>();
            return respone;
        }

        public async Task<AcademicYears> UpdateAsync(AcademicYears academicYears)
        {
            var data = await _httpClient.PostAsJsonAsync("api/AcademicYears/ReturnUpdate-AcademicYears", academicYears);
            var respone = await data.Content.ReadFromJsonAsync<AcademicYears>();
            return respone;
        }

        public async Task<PaginationModel<AcademicYears>> GetPagedAcademicYearsAsync(int pageNumber, int pageSize)
        {
            var response = await _httpClient.GetFromJsonAsync<PaginationModel<AcademicYears>>(
                $"api/academicYears?pageNumber={pageNumber}&pageSize={pageSize}");
            return response;
        }
    }
}
