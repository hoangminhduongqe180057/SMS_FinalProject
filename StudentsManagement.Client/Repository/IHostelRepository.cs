using StudentsManagement.Shared.Models;

namespace StudentsManagement.Client.Repository
{
    public interface IHostelRepository
    {
            Task<Hostel> AddAsync(Hostel Hostel);
            Task<Hostel> UpdateAsync(Hostel Hostel);
            Task<Hostel> DeleteAsync(int HostelId);
            Task<Hostel> GetByIdAsync(int HostelId);
            Task<List<Hostel>> GetAllAsync();
            Task<PaginationModel<Hostel>> GetPagedHostelsAsync(int pageNumber, int pageSize);
    }
}
