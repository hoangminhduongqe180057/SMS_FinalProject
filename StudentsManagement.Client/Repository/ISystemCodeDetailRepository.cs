using StudentsManagement.Shared.Models;

namespace StudentsManagement.Client.Repository
{
    public interface ISystemCodeDetailRepository
    {
        Task<SystemCodeDetail> AddAsync(SystemCodeDetail country);
        Task<SystemCodeDetail> UpdateAsync(SystemCodeDetail country);
        Task<SystemCodeDetail> DeleteAsync(int countryId);
        Task<SystemCodeDetail> GetByIdAsync(int countryId);
        Task<List<SystemCodeDetail>> GetAllAsync();
        Task<List<SystemCodeDetail>> GetByCodeAsync(string code);
        Task<PaginationModel<SystemCodeDetail>> GetPagedSystemCodeDetailsAsync(int pageNumber, int pageSize);
        Task<SystemCodeDetail> GetByStatusCodeAsync(string code, string statusCode);
    }
}
