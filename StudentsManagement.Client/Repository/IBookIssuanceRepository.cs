using StudentsManagement.Shared.Models;

namespace StudentsManagement.Client.Repository
{
    public interface IBookIssuanceRepository
    {
        Task<BookIssuance> AddAsync(BookIssuance bookIssuance);
        Task<BookIssuance> UpdateAsync(BookIssuance bookIssuance);
        Task<BookIssuance> DeleteAsync(int bookIssuanceId);
        Task<BookIssuance> GetByIdAsync(int bookIssuanceId);
        Task<List<BookIssuance>> GetAllAsync();
        Task<BookIssuance> ReturnBookUpdateAsync(BookIssuance bookIssuance);
        Task<PaginationModel<BookIssuance>> GetPagedBookIssuancesAsync(int pageNumber, int pageSize);
        Task<PaginationModel<BookIssuance>> GetPagedBookReturnedAsync(int pageNumber, int pageSize);
    }
}
