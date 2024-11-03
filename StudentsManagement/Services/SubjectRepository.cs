using Microsoft.EntityFrameworkCore;
using StudentsManagement.Client.Repository;
using StudentsManagement.Data;
using StudentsManagement.Shared.Models;
using System.Text;

namespace StudentsManagement.Services
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly ApplicationDbContext _context;
        public SubjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Subject> AddAsync(Subject subject)
        {
            subject.CreatedOn = DateTime.Now;
            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();

            return subject;
        }

        public async Task<Subject> DeleteAsync(int subjectId)
        {
            var subject = await _context.Subjects.Where(x => x.Id == subjectId).FirstOrDefaultAsync();
            if (subject == null) return null;

            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();

            return subject;
        }

        public async Task<List<Subject>> GetAllAsync()
        {
            var subjects = await _context.Subjects.ToListAsync();

            return subjects;
        }

        public async Task<Subject> GetByIdAsync(int subjectId)
        {
            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Id == subjectId);

            return subject;
        }

        public async Task<Subject> UpdateAsync(Subject subject)
        {
            if (subject == null) return null;


            var newsubject = _context.Subjects.Update(subject).Entity;
            await _context.SaveChangesAsync();

            return newsubject;
        }

        public async Task<PaginationModel<Subject>> GetPagedSubjectsAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _context.Subjects.CountAsync();
            var subjects = await _context.Subjects
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationModel<Subject>
            {
                Items = subjects,
                TotalItems = totalItems,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<byte[]> ExportToCsvAsync()
        {
            var subjects = await GetAllAsync();
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Id,Subject Name,Description,Create By,Create On");
            foreach (var subject in subjects)
            {
                csvBuilder.AppendLine($"{subject.Id},{subject.Name},{subject.Description},{subject.CreatedById},{subject.CreatedOn}");
            }
            // Convert StringBuilder to byte array with UTF-8 encoding and BOM
            var preamble = Encoding.UTF8.GetPreamble();  // UTF-8 BOM
            var csvContent = Encoding.UTF8.GetBytes(csvBuilder.ToString());

            // Combine BOM and CSV content into one byte array
            var finalContent = preamble.Concat(csvContent).ToArray();
            return finalContent;
        }

        public async Task<PaginationModel<Subject>> GetPagedSubjectsAsync(int pageNumber, int pageSize, SearchParameters searchParameters = null)
        {
            var query = _context.Subjects
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
            }

            // Áp dụng sắp xếp
            if (!string.IsNullOrEmpty(searchParameters.SortField))
            {
                switch (searchParameters.SortField)
                {
                    case "SubjectName":
                        query = searchParameters.SortAscending
                            ? query.OrderBy(s => s.Name)
                            : query.OrderByDescending(s => s.Name);
                        break;
                    case "Description":
                        query = searchParameters.SortAscending
                            ? query.OrderBy(s => s.Description)
                            : query.OrderByDescending(s => s.Description);
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
            var subjects = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationModel<Subject>
            {
                Items = subjects,
                TotalItems = totalItems,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
