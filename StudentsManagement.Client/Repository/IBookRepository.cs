using StudentsManagement.Shared.Models;

namespace StudentsManagement.Client.Repository
{
    public interface IBookRepository
    {
        Task<Book> AddAsync(Book book);
        Task<Book> UpdateAsync(Book book);
        Task<Book> DeleteAsync(int bookId);
        Task<Book> GetByIdAsync(int bookId);
        Task<List<Book>> GetAllAsync();
        Task<PaginationModel<Book>> GetPagedBooksAsync(int pageNumber, int pageSize);
        Task<byte[]> ExportBooksToCsvAsync();
        Task<PaginationModel<Book>> GetPagedBooksAsync(int pageNumber, int pageSize, SearchParameters searchParameters = null);
        Task<bool> ImportBooksAsync(Stream fileStream);
    }
}
