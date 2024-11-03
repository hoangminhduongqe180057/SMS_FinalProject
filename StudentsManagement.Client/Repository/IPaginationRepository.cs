using StudentsManagement.Shared.Models;

namespace StudentsManagement.Client.Repository
{
    public interface IPaginationRepository
    {
        Task<PaginationModel<T>> GetPagedDataAsync<T>(IQueryable<T> query, int pageNumber, int pageSize) where T : class;
    }
}