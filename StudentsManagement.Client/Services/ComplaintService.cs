using StudentsManagement.Client.Repository;
using StudentsManagement.Shared.Models;
using System.Net.Http.Json;

namespace StudentsManagement.Client.Services
{
    public class ComplaintService : IComplaintRepository
    {
        private readonly HttpClient _httpClient;
        public ComplaintService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Complaint> AddAsync(Complaint complaint)
        {
            var data = await _httpClient.PostAsJsonAsync("api/Complaints/Add-Complaint", complaint);
            var respone = await data.Content.ReadFromJsonAsync<Complaint>();
            return respone;
        }

        public async Task<Complaint> DeleteAsync(int complaintId)
        {
            var data = await _httpClient.PostAsJsonAsync("api/Complaints/Delete-Complaint", complaintId);
            var respone = await data.Content.ReadFromJsonAsync<Complaint>();
            return respone;
        }

        public async Task<List<Complaint>> GetAllAsync()
        {
            var data = await _httpClient.GetAsync("api/Complaints/All-Complaints");
            var respone = await data.Content.ReadFromJsonAsync<List<Complaint>>();
            return respone;
        }

        public async Task<Complaint> GetByIdAsync(int complaintId)
        {
            var data = await _httpClient.GetAsync($"api/Complaints/Single-Complaint/{complaintId}");
            var respone = await data.Content.ReadFromJsonAsync<Complaint>();
            return respone;
        }

        public async Task<PaginationModel<Complaint>> GetPagedComplaintsAsync(int pageNumber, int pageSize)
        {
            var response = await _httpClient.GetFromJsonAsync<PaginationModel<Complaint>>(
                $"api/Complaints?pageNumber={pageNumber}&pageSize={pageSize}");
            return response;
        }

        public async Task<Complaint> UpdateAsync(Complaint complaint)
        {
            var data = await _httpClient.PostAsJsonAsync("api/Complaints/Update-Complaint", complaint);
            var respone = await data.Content.ReadFromJsonAsync<Complaint>();
            return respone;
        }
    }
}
