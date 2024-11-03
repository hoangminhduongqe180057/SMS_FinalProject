using StudentsManagement.Shared.Models;

namespace StudentsManagement.Client.Repository
{
    public interface IDepartmentRepository
    {
        Task<Department> AddAsync(Department department);
        Task<Department> UpdateAsync(Department department);
        Task<Department> DeleteAsync(int departmentId);
        Task<Department> GetByIdAsync(int departmentId);
        Task<List<Department>> GetAllAsync();
        Task<PaginationModel<Department>> GetPagedDepartmentsAsync(int pageNumber, int pageSize);
        Task<byte[]> ExportToCsvAsync();
        Task<PaginationModel<Department>> GetPagedDepartmentsAsync(int pageNumber, int pageSize, SearchParameters searchParameters = null);
    }
}
