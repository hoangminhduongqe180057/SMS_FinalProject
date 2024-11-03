using StudentsManagement.Shared.Models;

namespace StudentsManagement.Client.Repository
{
    public interface IComplaintNoteRepository
    {
        Task<ComplaintNote> AddAsync(ComplaintNote complaintNote);
        Task<ComplaintNote> UpdateAsync(ComplaintNote complaintNote);
        Task<ComplaintNote> DeleteAsync(int complaintNoteId);
        Task<ComplaintNote> GetByIdAsync(int complaintNoteId);
        Task<List<ComplaintNote>> GetAllAsync();
        Task<PaginationModel<ComplaintNote>> GetPagedComplaintNotesAsync(int pageNumber, int pageSize);
    }
}
