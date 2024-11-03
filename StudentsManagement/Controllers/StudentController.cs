using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentsManagement.Client.Repository;
using StudentsManagement.Data;
using StudentsManagement.Shared.Models;
using StudentsManagement.Shared.StudentRepository;

namespace StudentsManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IStudentRepository _studentRepository;
        public StudentController(ApplicationDbContext context, IStudentRepository studentRepository)
        {
            this._context = context;
            _studentRepository = studentRepository;
        }


        [HttpGet("All-Students")]
        public async Task<ActionResult<List<Student>>> GetAllStudentsAsync()
        {
            var students = await _context.Students.ToListAsync();
            return Ok(students);
        }


        [HttpGet("Single-Student/{id}")]
        public async Task<ActionResult<Student>> GetSingleStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);

            return Ok(student);
        }

        [HttpPost("Add-Student")]
        public async Task<IActionResult> AddNewStudentAsync([FromBody] Student student)
        {
            try
            {
                var newStudent = await _context.Students.AddAsync(student);
                return Ok(newStudent);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpDelete("DeleteStudent/{id}")]
        public async Task<ActionResult<Student>> DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpPut("Update-Student")]
        public async Task<IActionResult> UpdateSingleStudent(int id, Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }

        [HttpGet("Total-Students")]
        public async Task<ActionResult<int>> GetTotalStudentsCountAsync()
        {
            var totalStudents = await _context.Students.AsNoTracking().CountAsync();
            return Ok(totalStudents);
        }

        [HttpGet("Search")]
        public async Task<ActionResult<List<Student>>> SearchStudentsAsync(
            [FromQuery] string registrationNo,
            [FromQuery] string name,
            [FromQuery] string email,
            [FromQuery] string phoneNumber,
            [FromQuery] string classes)
        {
            var query = _context.Students
                .Include(s => s.Gender)
                .Include(s => s.Country)
                .Include(s => s.Class)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(registrationNo))
                query = query.Where(s => s.RegistrationNo.Contains(registrationNo));

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(s => (s.FullName).Contains(name));

            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(s => s.EmailAddress.Contains(email));

            if (!string.IsNullOrWhiteSpace(phoneNumber))
                query = query.Where(s => s.PhoneNumber.Contains(phoneNumber));

            if (!string.IsNullOrWhiteSpace(classes))
                query = query.Where(s => s.Class.Description.Contains(classes));

            var students = await query.ToListAsync();
            return Ok(students);
        }

        [HttpPost("Import-Students")]

        public async Task<IActionResult> ImportStudents(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file provided.");
            }

            using var stream = file.OpenReadStream();
            bool result = await _studentRepository.ImportStudentsAsync(stream);

            return result ? Ok("Students imported successfully.") : StatusCode(500, "Failed to import students.");
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PaginationModel<Student>>> GetPagedStudentsAsync(
            [FromQuery] int pageNumber, [FromQuery] int pageSize,
            [FromQuery] string? registrationNo, [FromQuery] string? name,
            [FromQuery] string? email, [FromQuery] string? phoneNumber,
            [FromQuery] string? @class, [FromQuery] string? sortField,
            [FromQuery] bool sortAscending = true)
        {
            var searchParameters = new SearchParameters
            {
                RegistrationNo = registrationNo,
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber,
                Class = @class,
                SortField = sortField,
                SortAscending = sortAscending
            };

            var result = await _studentRepository.GetPagedStudentsAsync(pageNumber, pageSize, searchParameters);
            return Ok(result);
        }
    }
}
