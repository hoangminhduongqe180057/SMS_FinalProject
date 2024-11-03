using StudentsManagement.Shared.Models;

namespace StudentsManagement.Client.Repository
{
    public interface ISubjectRepository
    {
        Task<Subject> AddAsync(Subject subject);
        Task<Subject> UpdateAsync(Subject subject);
        Task<Subject> DeleteAsync(int subjectId);
        Task<Subject> GetByIdAsync(int subjectId);
        Task<List<Subject>> GetAllAsync();
        Task<PaginationModel<Subject>> GetPagedSubjectsAsync(int pageNumber, int pageSize);
        Task<byte[]> ExportToCsvAsync();
        Task<PaginationModel<Subject>> GetPagedSubjectsAsync(int pageNumber, int pageSize, SearchParameters searchParameters = null);
    }
}
