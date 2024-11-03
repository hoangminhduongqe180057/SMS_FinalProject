using StudentsManagement.Shared.Models;

namespace StudentsManagement.Client.Repository
{
    public interface IComplaintRepository
    {
        Task<Complaint> AddAsync(Complaint complaint);
        Task<Complaint> UpdateAsync(Complaint complaint);
        Task<Complaint> DeleteAsync(int complaintId);
        Task<Complaint> GetByIdAsync(int complaintId);
        Task<List<Complaint>> GetAllAsync();
        Task<PaginationModel<Complaint>> GetPagedComplaintsAsync(int pageNumber, int pageSize);
    }
}
