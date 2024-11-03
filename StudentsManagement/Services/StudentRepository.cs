using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentsManagement.Client.Repository;
using StudentsManagement.Data;
using StudentsManagement.Shared.Models;
using StudentsManagement.Shared.StudentRepository;
using System.Text;

namespace StudentsManagement.Services
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task<Student> AddStudentAsync(Student student)
        {
            // Kiểm tra mã sinh viên có trùng lặp không
            if (await _context.Students.AnyAsync(s => s.RegistrationNo == student.RegistrationNo && s.Id != student.Id))
            {
                throw new Exception("Registration number must be unique.");
            }

            // Kiểm tra email có trùng lặp không
            if (await _context.Students.AnyAsync(s => s.EmailAddress == student.EmailAddress && s.Id != student.Id))
            {
                throw new Exception("Email must be unique.");
            }

            // Kiểm tra số điện thoại có trùng lặp không
            if (await _context.Students.AnyAsync(s => s.PhoneNumber == student.PhoneNumber && s.Id != student.Id))
            {
                throw new Exception("Phone number must be unique.");
            }
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return student;
        }


        public async Task<Student> DeleteStudentAsync(int studentId)
        {
            var student = await _context.Students.Where(x => x.Id == studentId).FirstOrDefaultAsync();
            if (student == null) return null;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return student;
        }


        public async Task<List<Student>> GetAllStudentsAsync()
        {
            var students = await _context.Students.Include(s => s.Gender).Include(s => s.Country).Include(c => c.Class).ToListAsync();

            return students;
        }

        public async Task<Student> GetStudentByIdAsync(int studentId)
        {
            var student = await _context.Students
        .Include(s => s.Gender)
        .Include(s => s.Country)
        .Include(c => c.Class)
        .FirstOrDefaultAsync(s => s.Id == studentId);

            return student;
        }

        public async Task<Student> UpdateStudentAsync(Student student)
        {
            if (student == null) return null;

            // Kiểm tra email có trùng lặp không
            if (await _context.Students.AnyAsync(s => s.EmailAddress == student.EmailAddress && s.Id != student.Id))
            {
                throw new Exception("Email must be unique.");
            }

            // Kiểm tra số điện thoại có trùng lặp không
            if (await _context.Students.AnyAsync(s => s.PhoneNumber == student.PhoneNumber && s.Id != student.Id))
            {
                throw new Exception("Phone number must be unique.");
            }

            var newstudent = _context.Students.Update(student).Entity;
            await _context.SaveChangesAsync();

            return newstudent;
        }
        public async Task<PaginationModel<Student>> GetPagedStudentsAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _context.Students.CountAsync();
            var students = await _context.Students
                .Include(s => s.Gender)
                .Include(s => s.Country)
                .Include(c => c.Class)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationModel<Student>
            {
                Items = students,
                TotalItems = totalItems,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<int> GetTotalStudentsCountAsync()
        {
            return await _context.Students.CountAsync();
        }

        public async Task<byte[]> ExportToCsvAsync()
        {
            var students = await GetAllStudentsAsync();
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Register No,Full Name,Gender,Date of birth,Email,PhoneNumber,Country");
            foreach (var student in students)
            {
                csvBuilder.AppendLine($"{student.RegistrationNo},{student.FullName},{student.Gender.Description},{student.DOB.ToShortDateString()},{student.EmailAddress},{student.PhoneNumber},{student.Country.Name}");
            }
            // Convert StringBuilder to byte array with UTF-8 encoding and BOM
            var preamble = Encoding.UTF8.GetPreamble();  // UTF-8 BOM
            var csvContent = Encoding.UTF8.GetBytes(csvBuilder.ToString());

            // Combine BOM and CSV content into one byte array
            var finalContent = preamble.Concat(csvContent).ToArray();
            return finalContent;
        }

        public async Task<PaginationModel<Student>> GetPagedStudentsAsync(int pageNumber, int pageSize, SearchParameters searchParameters = null)
        {
            var query = _context.Students
                .Include(s => s.Gender)
                .Include(s => s.Country)
                .Include(s => s.Class)
                .OrderBy(s => s.Id)
                .AsQueryable();

            // Apply search filters if provided
            if (searchParameters != null)
            {
                if (!string.IsNullOrEmpty(searchParameters.RegistrationNo))
                    query = query.Where(s => s.RegistrationNo.Contains(searchParameters.RegistrationNo));

                if (!string.IsNullOrEmpty(searchParameters.Name))
                {
                    string normalizedSearchName = searchParameters.Name.Trim().ToLower();  // Loại bỏ khoảng trắng và chuyển thành chữ thường
                    query = query.Where(s =>
                        (s.FirstName + " " + s.LastName).Trim().ToLower().Contains(normalizedSearchName));  // So sánh chữ thường và loại bỏ khoảng trắng thừa
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
                    case "DOB":
                        query = searchParameters.SortAscending
                            ? query.OrderBy(s => s.DOB)
                            : query.OrderByDescending(s => s.DOB);
                        break;
                    case "Class":
                        query = searchParameters.SortAscending
                            ? query.OrderBy(s => s.Class.Description)
                            : query.OrderByDescending(s => s.Class.Description);
                        break;
                    case "Gender":
                        query = searchParameters.SortAscending
                            ? query.OrderBy(s => s.Gender.Description)
                            : query.OrderByDescending(s => s.Gender.Description);
                        break;
                    default:
                        query = searchParameters.SortAscending
                            ? query.OrderBy(s => s.RegistrationNo)
                            : query.OrderByDescending(s => s.RegistrationNo);
                        break;
                }
            }

            // Pagination
            var totalItems = await query.CountAsync();
            var students = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationModel<Student>
            {
                Items = students,
                TotalItems = totalItems,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<bool> ImportStudentsAsync(Stream fileStream)
        {
            var students = new List<Student>();

            using var reader = new StreamReader(fileStream, Encoding.UTF8, leaveOpen: true);

            string line;
            // Optionally skip header row
            await reader.ReadLineAsync();

            while ((line = await reader.ReadLineAsync()) != null)
            {
                var values = line.Split(',');

                if (values.Length >= 11) // Ensure enough fields are present
                {
                    try
                    {
                        var student = new Student
                        {
                            RegistrationNo = values[0],
                            FirstName = values[1],
                            MiddleName = values.Length > 2 ? values[2] : null,
                            LastName = values[3],
                            EmailAddress = values[4],
                            Address = values.Length > 5 ? values[5] : null,
                            PhoneNumber = values[6],
                            CountryId = int.Parse(values[7]),
                            DOB = DateTime.Parse(values[8]),
                            GenderId = int.Parse(values[9]),
                            ClassId = int.Parse(values[10]),
                        };
                        students.Add(student);
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

            if (students.Any())
            {
                _context.Students.AddRange(students);
                await _context.SaveChangesAsync();  // Save all imported records asynchronously
                Console.WriteLine($"{students.Count} students imported successfully.");
                return true;
            }

            Console.WriteLine("No valid students to import.");
            return false;
        }


    }
}
