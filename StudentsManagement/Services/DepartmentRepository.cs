using Microsoft.EntityFrameworkCore;
using StudentsManagement.Client.Repository;
using StudentsManagement.Data;
using StudentsManagement.Shared.Models;
using System.Text;

namespace StudentsManagement.Services
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;
        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Department> AddAsync(Department department)
        {
            department.CreatedOn = DateTime.Now;
            if (department == null) return null;
            var data = _context.Departments.Add(department).Entity;
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<Department> DeleteAsync(int departmentId)
        {

            var data = await _context.Departments.Where(x => x.Id == departmentId).FirstOrDefaultAsync();
            if (data == null) return null;
            _context.Departments.Remove(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<List<Department>> GetAllAsync()
        {
            var data = await _context.Departments.ToListAsync();
            return data;
        }

        public async Task<Department> GetByIdAsync(int departmentId)
        {
            var data = await _context.Departments.FirstOrDefaultAsync(x => x.Id == departmentId);
            return data;
        }

        public async Task<Department> UpdateAsync(Department department)
        {
            if (department == null) return null;
            var data = _context.Departments.Update(department).Entity;
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<PaginationModel<Department>> GetPagedDepartmentsAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _context.Departments.CountAsync();
            var departments = await _context.Departments
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationModel<Department>
            {
                Items = departments,
                TotalItems = totalItems,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<byte[]> ExportToCsvAsync()
        {
            var departments = await GetAllAsync();
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Id No,Department Name,Department Code,Create By,Create On");
            foreach (var department in departments)
            {
                csvBuilder.AppendLine($"{department.Id},{department.Name},{department.Code},{department.CreatedById},{department.CreatedOn}}");
            }
            // Convert StringBuilder to byte array with UTF-8 encoding and BOM
            var preamble = Encoding.UTF8.GetPreamble();  // UTF-8 BOM
            var csvContent = Encoding.UTF8.GetBytes(csvBuilder.ToString());

            // Combine BOM and CSV content into one byte array
            var finalContent = preamble.Concat(csvContent).ToArray();
            return finalContent;
        }

        public async Task<PaginationModel<Department>> GetPagedDepartmentsAsync(int pageNumber, int pageSize, SearchParameters searchParameters = null)
        {
            var query = _context.Departments
                .OrderBy(s => s.Id)
                .AsQueryable();

            // Apply search filters if provided
            if (searchParameters != null)
            {
                if (!string.IsNullOrEmpty(searchParameters.Name))
                {
                    var normalizedSearchName = searchParameters.Name.ToLower();
                    query = query.Where(s => s.Name.Trim().ToLower().Contains(normalizedSearchName));
                }

                if (!string.IsNullOrEmpty(searchParameters.Code))
                {
                    var normalizedSearchName = searchParameters.Code.ToLower();
                    query = query.Where(s => s.Code.Trim().ToLower().Contains(normalizedSearchName));
                }
            }

            // Áp dụng sắp xếp
            if (!string.IsNullOrEmpty(searchParameters.SortField))
            {
                switch (searchParameters.SortField)
                {
                    case "DepartmentName":
                        query = searchParameters.SortAscending
                            ? query.OrderBy(s => s.Name)
                            : query.OrderByDescending(s => s.Name);
                        break;
                    case "Code":
                        query = searchParameters.SortAscending
                            ? query.OrderBy(s => s.Code)
                            : query.OrderByDescending(s => s.Code);
                        break;
                    case "CreateBy":
                        query = searchParameters.SortAscending
                            ? query.OrderBy(s => s.CreatedById)
                            : query.OrderByDescending(s => s.CreatedById);
                        break;
                    case "CreateOn":
                        query = searchParameters.SortAscending
                            ? query.OrderBy(s => s.CreatedOn)
                            : query.OrderByDescending(s => s.CreatedOn);
                        break;
                    default:
                        query = searchParameters.SortAscending
                            ? query.OrderBy(s => s.Id)
                            : query.OrderByDescending(s => s.Id);
                        break;
                }
            }

            // Pagination
            var totalItems = await query.CountAsync();
            var departments = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationModel<Department>
            {
                Items = departments,
                TotalItems = totalItems,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
