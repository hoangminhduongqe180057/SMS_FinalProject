using StudentsManagement.Client.Repository;
using StudentsManagement.Shared.Models;
using System.Net.Http.Json;

namespace StudentsManagement.Client.Services
{
    public class ComplaintNoteService : IComplaintNoteRepository
    {
        private readonly HttpClient _httpClient;
        public ComplaintNoteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ComplaintNote> AddAsync(ComplaintNote complaintNote)
        {
            var data = await _httpClient.PostAsJsonAsync("api/ComplaintNotes/Add-ComplaintNote", complaintNote);
            var respone = await data.Content.ReadFromJsonAsync<ComplaintNote>();
            return respone;
        }

        public async Task<ComplaintNote> DeleteAsync(int complaintNoteId)
        {
            var data = await _httpClient.PostAsJsonAsync("api/ComplaintNotes/Delete-ComplaintNote", complaintNoteId);
            var respone = await data.Content.ReadFromJsonAsync<ComplaintNote>();
            return respone;
        }

        public async Task<List<ComplaintNote>> GetAllAsync()
        {
            var data = await _httpClient.GetAsync("api/ComplaintNotes/All-ComplaintNotes");
            var respone = await data.Content.ReadFromJsonAsync<List<ComplaintNote>>();
            return respone;
        }

        public async Task<ComplaintNote> GetByIdAsync(int complaintNoteId)
        {
            var data = await _httpClient.GetAsync($"api/ComplaintNotes/Single-ComplaintNote/{complaintNoteId}");
            var respone = await data.Content.ReadFromJsonAsync<ComplaintNote>();
            return respone;
        }

        public async Task<PaginationModel<ComplaintNote>> GetPagedComplaintNotesAsync(int pageNumber, int pageSize)
        {
            var response = await _httpClient.GetFromJsonAsync<PaginationModel<ComplaintNote>>(
                $"api/ComplaintNotes?pageNumber={pageNumber}&pageSize={pageSize}");
            return response;
        }

        public async Task<ComplaintNote> UpdateAsync(ComplaintNote complaintNote)
        {
            var data = await _httpClient.PostAsJsonAsync("api/ComplaintNotes/Update-ComplaintNote", complaintNote);
            var respone = await data.Content.ReadFromJsonAsync<ComplaintNote>();
            return respone;
        }
    }
}
