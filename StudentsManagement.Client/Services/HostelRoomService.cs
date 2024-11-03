using StudentsManagement.Client.Repository;
using StudentsManagement.Shared.Models;
using System.Net.Http.Json;

namespace StudentsManagement.Client.Services
{
    public class HostelRoomService : IHostelRoomRepository
    {
        private readonly HttpClient _httpClient;
        public HostelRoomService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<HostelRoom> AddAsync(HostelRoom Hostelroom)
        {
            var data = await _httpClient.PostAsJsonAsync("api/HostelRooms/Add-HostelRoom", Hostelroom);
            var respone = await data.Content.ReadFromJsonAsync<HostelRoom>();
            return respone;
        }

        public async Task<HostelRoom> DeleteAsync(int HostelroomId)
        {
            var data = await _httpClient.PostAsJsonAsync("api/HostelRooms/Delete-HostelRoom", HostelroomId);
            var respone = await data.Content.ReadFromJsonAsync<HostelRoom>();
            return respone;
        }

        public async Task<List<HostelRoom>> GetAllAsync()
        {
            var data = await _httpClient.GetAsync("api/HostelRooms/All-HostelRooms");
            var respone = await data.Content.ReadFromJsonAsync<List<HostelRoom>>();
            return respone;
        }

        public async Task<HostelRoom> GetByIdAsync(int HostelroomId)
        {
            var data = await _httpClient.GetAsync($"api/HostelRooms/Single-HostelRoom/{HostelroomId}");
            var respone = await data.Content.ReadFromJsonAsync<HostelRoom>();
            return respone;
        }

        public async Task<PaginationModel<HostelRoom>> GetPagedHostelRoomsAsync(int pageNumber, int pageSize)
        {
            var response = await _httpClient.GetFromJsonAsync<PaginationModel<HostelRoom>>(
                $"api/HostelRooms?pageNumber={pageNumber}&pageSize={pageSize}");
            return response;
        }

        public async Task<HostelRoom> UpdateAsync(HostelRoom Hostelroom)
        {
            var data = await _httpClient.PostAsJsonAsync("api/HostelRooms/ReturnUpdate-HostelRooms", Hostelroom);
            var respone = await data.Content.ReadFromJsonAsync<HostelRoom>();
            return respone;
        }
    }
}
