using StudentsManagement.Shared.Models;

namespace StudentsManagement.Client.Repository
{
    public interface IAcademicYearRepository
    {
        Task<AcademicYears> AddAsync(AcademicYears academicYears);
        Task<AcademicYears> UpdateAsync(AcademicYears academicYears);
        Task<AcademicYears> DeleteAsync(int academicYearsId);
        Task<AcademicYears> GetByIdAsync(int academicYearsId);
        Task<List<AcademicYears>> GetAllAsync();
        Task<PaginationModel<AcademicYears>> GetPagedAcademicYearsAsync(int pageNumber, int pageSize);
    }
}
