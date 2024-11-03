using StudentsManagement.Shared.Models;

namespace StudentsManagement.Client.Repository
{
    public interface ISystemCodeRepository
    {
        Task<SystemCode> AddAsync(SystemCode country);
        Task<SystemCode> UpdateAsync(SystemCode country);
        Task<SystemCode> DeleteAsync(int countryId);
        Task<SystemCode> GetByIdAsync(int countryId);
        Task<List<SystemCode>> GetAllAsync();
        Task<PaginationModel<SystemCode>> GetPagedSystemCodesAsync(int pageNumber, int pageSize);
    }
}
