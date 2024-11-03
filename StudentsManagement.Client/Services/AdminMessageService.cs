using StudentsManagement.Shared.Models;
using System.Net.Http.Json;

namespace StudentsManagement.Client.Services
{
    public class AdminMessageService
    {
        private readonly HttpClient _httpClient;

        public AdminMessageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<AdminMessage>> GetMessagesAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<AdminMessage>>("api/AdminMessages");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
                return new List<AdminMessage>(); // Trả về danh sách rỗng nếu lỗi xảy ra
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return new List<AdminMessage>(); // Trả về danh sách rỗng nếu lỗi xảy ra
            }
        }

        public async Task<bool> SendMessageAsync(AdminMessage message)
        {
            var response = await _httpClient.PostAsJsonAsync("api/AdminMessages", message);
            return response.IsSuccessStatusCode;
        }
    }
}
