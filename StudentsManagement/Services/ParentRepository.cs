using Microsoft.EntityFrameworkCore;
using StudentsManagement.Client.Repository;
using StudentsManagement.Data;
using StudentsManagement.Shared.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace StudentsManagement.Services
{
    public class ParentRepository : IParentRepository
    {
        private readonly ApplicationDbContext _context;

        public ParentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Parent> AddAsync(Parent parent)
        {
            // Kiểm tra email có trùng lặp không
            if (await _context.Parents.AnyAsync(p => p.EmailAddress == parent.EmailAddress && p.Id != parent.Id))
            {
                throw new Exception("Email must be unique.");
            }

            // Kiểm tra số điện thoại có trùng lặp không
            if (await _context.Parents.AnyAsync(p => p.PhoneNumber == parent.PhoneNumber && p.Id != parent.Id))
            {
                throw new Exception("Phone number must be unique.");
            }

            _context.Parents.Add(parent);
            await _context.SaveChangesAsync();

            return parent;
        }

        public async Task<Parent> DeleteAsync(int parentId)
        {
            var parent = await _context.Parents.Where(x => x.Id == parentId).FirstOrDefaultAsync();
            if (parent == null) return null;
            _context.Parents.Remove(parent);
            await _context.SaveChangesAsync();
            return parent;
        }

        public async Task<List<Parent>> GetAllAsync()
        {
            var data = await _context.Parents
                .Include(x => x.Student)
                .Include(s => s.Gender)
                .Include(p => p.ParentType)
                .Include(c => c.Class)
                .ToListAsync();

            return data;
        }

        public async Task<Parent> GetByIdAsync(int parentId)
        {
            var data = await _context.Parents.Include(x => x.Student)
                .Include(s => s.Gender)
                .Include(p => p.ParentType)
                .Include(c => c.Class)
                .Where(x => x.Id == parentId)
                .FirstOrDefaultAsync();
            if (data == null) return null;

            return data;
        }

        public async Task<Parent> UpdateAsync(Parent parent)
        {
            if (parent == null) return null;
            // Kiểm tra email có trùng lặp không
            if (await _context.Parents.AnyAsync(p => p.EmailAddress == parent.EmailAddress && p.Id != parent.Id))
            {
                throw new Exception("Email must be unique.");
            }

            // Kiểm tra số điện thoại có trùng lặp không
            if (await _context.Parents.AnyAsync(p => p.PhoneNumber == parent.PhoneNumber && p.Id != parent.Id))
            {
                throw new Exception("Phone number must be unique.");
            }
            var data = _context.Parents.Update(parent).Entity;
            await _context.SaveChangesAsync();

            return data;
        }

        public async Task<PaginationModel<Parent>> GetPagedParentsAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _context.Parents.CountAsync();
            var parents = await _context.Parents
                .Include(x => x.Student)
                .Include(s => s.Gender)
                .Include(p => p.ParentType)
                .Include(c => c.Class)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationModel<Parent>
            {
                Items = parents,
                TotalItems = totalItems,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<byte[]> ExportToCsvAsync()
        {
            var parents = await GetAllAsync();
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Id,Full Name,Phone Number,Email,Class");
            foreach (var parent in parents)
            {
                csvBuilder.AppendLine($"{parent.Id},{parent.FullName},{parent.PhoneNumber},{parent.EmailAddress},{parent.Class.Description}");
            }
            // Convert StringBuilder to byte array with UTF-8 encoding and BOM
            var preamble = Encoding.UTF8.GetPreamble();  // UTF-8 BOM
            var csvContent = Encoding.UTF8.GetBytes(csvBuilder.ToString());

            // Combine BOM and CSV content into one byte array
            var finalContent = preamble.Concat(csvContent).ToArray();
            return finalContent;
        }

        public async Task<PaginationModel<Parent>> GetPagedParentsAsync(int pageNumber, int pageSize, SearchParameters searchParameters = null)
        {
            var query = _context.Parents
                .Include(x => x.Student)
                .Include(s => s.Gender)
                .Include(p => p.ParentType)
                .Include(c => c.Class)
                .OrderBy(s => s.Id)
                .AsQueryable();

            // Apply search filters if provided
            if (searchParameters != null)
            {
                if (!string.IsNullOrEmpty(searchParameters.StudentName))
                {
                    var normalizedSearchName = searchParameters.StudentName.ToLower();
                    query = query.Where(s =>
                        (s.Student.FirstName + " " + s.Student.LastName).Trim().ToLower().Contains(normalizedSearchName));
                }


                if (!string.IsNullOrEmpty(searchParameters.Name))
                {
                    var normalizedSearchName = searchParameters.Name.Trim().ToLower();
                    query = query.Where(s =>
                        (s.FirstName + " " + s.LastName).Trim().ToLower().Contains(normalizedSearchName));
                }

                if (!string.IsNullOrEmpty(searchParameters.Email))
                    query = query.Where(s => s.EmailAddress.Contains(searchParameters.Email));

                if (!string.IsNullOrEmpty(searchParameters.PhoneNumber))
                    query = query.Where(s => s.PhoneNumber.Contains(searchParameters.PhoneNumber));

                if (!string.IsNullOrEmpty(searchParameters.Class))
                    query = query.Where(s => s.Class.Description.Contains(searchParameters.Class));
            }

            // Áp dụng sắp xếp
            if (!string.IsNullOrEmpty(searchParameters.SortField))
            {
                switch (searchParameters.SortField)
                {
                    case "FullName":
                        query = searchParameters.SortAscending
                            ? query.OrderBy(s => s.FirstName).ThenBy(s => s.LastName)
                            : query.OrderByDescending(s => s.FirstName).ThenByDescending(s => s.LastName);
                        break;
                    case "EmailAddress":
                        query = searchParameters.SortAscending
                            ? query.OrderBy(s => s.EmailAddress)
                            : query.OrderByDescending(s => s.EmailAddress);
                        break;
                    case "PhoneNumber":
                        query = searchParameters.SortAscending
                            ? query.OrderBy(s => s.PhoneNumber)
                            : query.OrderByDescending(s => s.PhoneNumber);
                        break;
                    case "StudentName":
                        query = searchParameters.SortAscending
                            ? query.OrderBy(s => s.Student.FirstName).ThenBy(s => s.Student.LastName)
                            : query.OrderByDescending(s => s.Student.FirstName).ThenByDescending(s => s.Student.LastName);
                        break;
                    case "Class":
                        query = searchParameters.SortAscending
                            ? query.OrderBy(s => s.Class.Description)
                            : query.OrderByDescending(s => s.Class.Description);
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
            var parents = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationModel<Parent>
            {
                Items = parents,
                TotalItems = totalItems,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
