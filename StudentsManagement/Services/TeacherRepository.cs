using Microsoft.EntityFrameworkCore;
using StudentsManagement.Client.Repository;
using StudentsManagement.Data;
using StudentsManagement.Shared.Models;
using System.Text;

namespace StudentsManagement.Services
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly ApplicationDbContext _context;

        public TeacherRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Teacher> AddAsync(Teacher teacher)
        {
            // Kiểm tra email có trùng lặp không
            if (await _context.Teachers.AnyAsync(t => t.EmailAddress == teacher.EmailAddress && t.Id != teacher.Id))
            {
                throw new Exception("Email must be unique.");
            }

            // Kiểm tra số điện thoại có trùng lặp không
            if (await _context.Teachers.AnyAsync(t => t.PhoneNumber == teacher.PhoneNumber && t.Id != teacher.Id))
            {
                throw new Exception("Phone number must be unique.");
            }

            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();

            return teacher;
        }

        public async Task<Teacher> DeleteAsync(int teacherId)
        {
            var data = await _context.Teachers.Where(x => x.Id == teacherId).FirstOrDefaultAsync();
            if (data == null) return null;
            _context.Teachers.Remove(data);
            await _context.SaveChangesAsync();
            return data;

        }

        public async Task<List<Teacher>> GetAllAsync()
        {
            var data = await _context.Teachers
                .Include(s => s.Gender)
                .Include(x => x.MaritalStatus)
                .Include(d => d.Designation)
                .ToListAsync();
            return data;
        }

        public async Task<Teacher> GetByIdAsync(int teacherId)
        {
            var data = await _context.Teachers
                .Include(s => s.Gender)
                .Include(x => x.MaritalStatus)
                .Include(d => d.Designation)
                .Include(z => z.Designation)
                .Where(x => x.Id == teacherId)
                .FirstOrDefaultAsync();
            if (data == null) return null;
            return data;
        }

        public async Task<Teacher> UpdateAsync(Teacher teacher)
        {
            if (teacher == null) return null;
            // Kiểm tra email có trùng lặp không
            if (await _context.Teachers.AnyAsync(t => t.EmailAddress == teacher.EmailAddress && t.Id != teacher.Id))
            {
                throw new Exception("Email must be unique.");
            }

            // Kiểm tra số điện thoại có trùng lặp không
            if (await _context.Teachers.AnyAsync(t => t.PhoneNumber == teacher.PhoneNumber && t.Id != teacher.Id))
            {
                throw new Exception("Phone number must be unique.");
            }

            var data = _context.Teachers.Update(teacher).Entity;
            await _context.SaveChangesAsync();

            return data;
        }

        public async Task<PaginationModel<Teacher>> GetPagedTeachersAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _context.Teachers.CountAsync();  
            var teachers = await _context.Teachers
                .Include(s => s.Gender)
                .Include(x => x.MaritalStatus)
                .Include(z => z.Designation)
                .Skip((pageNumber - 1) * pageSize)  
                .Take(pageSize)            
                .ToListAsync();

            return new PaginationModel<Teacher>
            {
                Items = teachers,
                TotalItems = totalItems,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<int> GetTotalTeachersCountAsync()
        {
            return await _context.Teachers.CountAsync();
        }

        public async Task<byte[]> ExportToCsvAsync()
        {
            var teachers = await GetAllAsync();
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Full Name,Gender,Address,Email,PhoneNumber,Marital Status");
            foreach (var teacher in teachers)
            {
                csvBuilder.AppendLine($"{teacher.FullName},{teacher.Gender.Description},{teacher.Address},{teacher.EmailAddress},{teacher.PhoneNumber},{teacher.MaritalStatus.Description}");
            }
            // Convert StringBuilder to byte array with UTF-8 encoding and BOM
            var preamble = Encoding.UTF8.GetPreamble();  // UTF-8 BOM
            var csvContent = Encoding.UTF8.GetBytes(csvBuilder.ToString());

            // Combine BOM and CSV content into one byte array
            var finalContent = preamble.Concat(csvContent).ToArray();
            return finalContent;
        }

        public async Task<PaginationModel<Teacher>> GetPagedTeachersAsync(int pageNumber, int pageSize, SearchParameters searchParameters = null)
        {
            var query = _context.Teachers
                .Include(s => s.Gender)
                .Include(x => x.MaritalStatus)
                .Include(d => d.Designation)
                .OrderBy(s => s.Id)
                .AsQueryable();

            // Apply search filters if provided
            if (searchParameters != null)
            {
                if (!string.IsNullOrEmpty(searchParameters.Name))
                {
                    string normalizedSearchName = searchParameters.Name.Trim();
                    query = query.Where(s => (s.FirstName + " " + (s.MiddleName ?? "") + " " + s.LastName).Trim().Contains(searchParameters.Name));
                }

                if (!string.IsNullOrEmpty(searchParameters.Email))
                    query = query.Where(s => s.EmailAddress.Contains(searchParameters.Email));

                if (!string.IsNullOrEmpty(searchParameters.PhoneNumber))
                    query = query.Where(s => s.PhoneNumber.Contains(searchParameters.PhoneNumber));
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
                    case "Address":
                        query = searchParameters.SortAscending
                            ? query.OrderBy(s => s.Address)
                            : query.OrderByDescending(s => s.Address);
                        break;
                    case "Gender":
                        query = searchParameters.SortAscending
                            ? query.OrderBy(s => s.Gender.Description)
                            : query.OrderByDescending(s => s.Gender.Description);
                        break;
                    case "MaritalStatus":
                        query = searchParameters.SortAscending
                            ? query.OrderBy(s => s.MaritalStatus.Description)
                            : query.OrderByDescending(s => s.MaritalStatus.Description);
                        break;
                    case "PhoneNumber":
                        query = searchParameters.SortAscending
                            ? query.OrderBy(s => s.PhoneNumber)
                            : query.OrderByDescending(s => s.PhoneNumber);
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
            var teachers = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationModel<Teacher>
            {
                Items = teachers,
                TotalItems = totalItems,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<bool> ImportTeachersAsync(Stream fileStream)
        {
            var teachers = new List<Teacher>();

            using var reader = new StreamReader(fileStream, Encoding.UTF8, leaveOpen: true);

            string line;
            // Optionally skip header row
            await reader.ReadLineAsync();

            while ((line = await reader.ReadLineAsync()) != null)
            {
                var values = line.Split(',');

                if (values.Length >= 13) // Ensure enough fields are present
                {
                    try
                    {
                        // Try to parse necessary fields to ensure correct data type conversion
                        if (int.TryParse(values[4], out var genderId) &&
                            int.TryParse(values[7], out var maritalStatusId) &&
                            DateTime.TryParse(values[8], out var dob) &&
                            int.TryParse(values[12], out var designationId))
                        {
                            var teacher = new Teacher
                            {
                                FirstName = values[0],
                                MiddleName = values.Length > 1 ? values[1] : null,
                                LastName = values.Length > 2 ? values[2] : null,
                                EmailAddress = values[3],
                                GenderId = genderId,
                                PhoneNumber = values[5],
                                Address = values[6],
                                MaritalStatusId = maritalStatusId,
                                DOB = dob,
                                FacebookLink = values.Length > 9 ? values[9] : null,
                                TwitterLink = values.Length > 10 ? values[10] : null,
                                LinkedInLink = values.Length > 11 ? values[11] : null,
                                DesignationId = designationId,
                            };
                            teachers.Add(teacher);
                        }
                        else
                        {
                            Console.WriteLine("Failed to parse data for teacher.");
                        }
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"Format error on line: {ex.Message}");
                        continue;  // Skip invalid lines and log error
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing line: {ex.Message}");
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("Skipping line due to insufficient fields.");
                }
            }

            if (teachers.Any())
            {
                _context.Teachers.AddRange(teachers);
                await _context.SaveChangesAsync();  // Save all imported records asynchronously
                Console.WriteLine($"{teachers.Count} teachers imported successfully.");
                return true;
            }

            Console.WriteLine("No valid teacher to import.");
            return false;
        }

    }
}
