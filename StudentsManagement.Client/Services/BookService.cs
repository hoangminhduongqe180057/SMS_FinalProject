using Microsoft.JSInterop;
using StudentsManagement.Client.Repository;
using StudentsManagement.Shared.Models;
using System.Net.Http.Json;
using Microsoft.JSInterop;
using static System.Reflection.Metadata.BlobBuilder;
using System.Net.Http.Headers;

namespace StudentsManagement.Client.Services
{
    public class BookService : IBookRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        public BookService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }
        public async Task<Book> AddAsync(Book book)
        {
            var data = await _httpClient.PostAsJsonAsync("api/Books/Add-Book", book);
            var respone = await data.Content.ReadFromJsonAsync<Book>();
            return respone;
        }

        public async Task<Book> DeleteAsync(int bookId)
        {
            var data = await _httpClient.PostAsJsonAsync("api/Books/Delete-Book", bookId);
            var respone = await data.Content.ReadFromJsonAsync<Book>();
            return respone;
        }

        public async Task<List<Book>> GetAllAsync()
        {
            var data = await _httpClient.GetAsync("api/Books/All-Books");
            var respone = await data.Content.ReadFromJsonAsync<List<Book>>();
            return respone;
        }

        public async Task<Book> GetByIdAsync(int bookId)
        {
            var data = await _httpClient.GetAsync($"api/Books/Single-Book/{bookId}");
            var respone = await data.Content.ReadFromJsonAsync<Book>();
            return respone;
        }

        public async Task<Book> UpdateAsync(Book book)
        {
            var data = await _httpClient.PostAsJsonAsync("api/Books/Update-Book", book);
            var respone = await data.Content.ReadFromJsonAsync<Book>();
            return respone;
        }

        public async Task<PaginationModel<Book>> GetPagedBooksAsync(int pageNumber, int pageSize)
        {
            var response = await _httpClient.GetFromJsonAsync<PaginationModel<Book>>(
                $"api/book?pageNumber={pageNumber}&pageSize={pageSize}");
            return response;
        }

        public async Task<int> GetTotalBooksCountAsync()
        {
            var totalBooks = await _httpClient.GetFromJsonAsync<int>("api/Books/Total-Books");
            return totalBooks;
        }

        public async Task<byte[]> ExportBooksToCsvAsync()
        {
            var url = "/api/export/ExportBooksToCsv";
            var response = await _httpClient.GetAsync(url);
            var csvContent = await response.Content.ReadAsByteArrayAsync();
            await _jsRuntime.InvokeVoidAsync("downloadFile", "Books.csv", "text/csv", csvContent);
            return new byte[0];
        }

        public async Task<PaginationModel<Book>> GetPagedBooksAsync(int pageNumber, int pageSize, SearchParameters searchParameters = null)
        {
            var queryString = $"pageNumber={pageNumber}&pageSize={pageSize}";

            if (searchParameters != null)
            {
                if (!string.IsNullOrEmpty(searchParameters.Name))
                    queryString += $"&name={searchParameters.Name}";
                if (!string.IsNullOrEmpty(searchParameters.Author))
                    queryString += $"&author={searchParameters.Author}";
                if (!string.IsNullOrEmpty(searchParameters.Category))
                    queryString += $"&category={searchParameters.Category}";

                // Thêm tham số sắp xếp
                queryString += $"&sortField={searchParameters.SortField}&sortAscending={searchParameters.SortAscending}";
            }

            var response = await _httpClient.GetFromJsonAsync<PaginationModel<Book>>($"api/books/paged?{queryString}");
            return response;
        }

        public async Task<bool> ImportBooksAsync(Stream fileStream)
        {
            if (fileStream == null)
                return false;

            // Prepare MultipartFormDataContent with the provided Stream
            using var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(fileStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
            content.Add(streamContent, "file", "books_import.csv");

            // Send request to the backend
            var response = await _httpClient.PostAsync("api/Book/Import-Books", content);
            return response.IsSuccessStatusCode;
        }
    }
}
