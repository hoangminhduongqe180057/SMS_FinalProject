using StudentsManagement.Shared.Models;

namespace StudentsManagement.Client.Repository
{
    public interface IHostelRoomRepository
    {
        Task<HostelRoom> AddAsync(HostelRoom Hostelroom);
        Task<HostelRoom> UpdateAsync(HostelRoom Hostelroom);
        Task<HostelRoom> DeleteAsync(int HostelroomId);
        Task<HostelRoom> GetByIdAsync(int HostelroomId);
        Task<List<HostelRoom>> GetAllAsync();
        Task<PaginationModel<HostelRoom>> GetPagedHostelRoomsAsync(int pageNumber, int pageSize);
    }
}
