using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentsManagement.Client.Repository;
using StudentsManagement.Data;
using StudentsManagement.Services;
using StudentsManagement.Shared.Models;
using StudentsManagement.Shared.StudentRepository;

namespace StudentsManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ITeacherRepository _teacherRepository;

        public TeachersController(ApplicationDbContext context, ITeacherRepository teachertRepository)
        {
            _context = context;
            _teacherRepository = teachertRepository;
        }

        // GET: api/Teachers
        [HttpGet("All-Teachers")]
        public async Task<ActionResult<List<Teacher>>> GetAllTeachersAsyncs()
        {
            var teachers = await _context.Teachers.ToListAsync();
            return Ok(teachers);
        }

        // GET: api/Teachers/5
        [HttpGet("Single-Teacher/{id}")]
        public async Task<ActionResult<Teacher>> GetTeacherByIdAsync(int id)
        {
            var student = await _context.Teachers.FindAsync(id);

            return Ok(student);
        }

        // PUT: api/Teachers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Update-Teacher")]
        public async Task<IActionResult> AddNewTeacherAsync(Teacher teacher)
        {
            var updateTeacher = await _context.Teachers.AddAsync(teacher);

            return Ok(updateTeacher);
        }

        // POST: api/Teachers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Add-Teacher")]
        public async Task<IActionResult> UpdateSingleTeacher(int id, Teacher teacher)
        {
            if (id != teacher.Id)
            {
                return BadRequest();
            }

            _context.Entry(teacher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(id))
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

        // DELETE: api/Teachers/5
        [HttpDelete("DeleteTeacher/{id}")]
        public async Task<ActionResult<Student>> DeleteTeacherAsync(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TeacherExists(int id)
        {
            return _context.Teachers.Any(e => e.Id == id);
        }

        [HttpGet("Total-Teachers")]
        public async Task<ActionResult<int>> GetTotalTeachersCountAsync()
        {
            var totalTeachers = await _context.Teachers.CountAsync();
            return Ok(totalTeachers);
        }

        [HttpGet("Search")]
        public async Task<ActionResult<List<Teacher>>> SearchTeachersAsync(
            [FromQuery] string name,
            [FromQuery] string email,
            [FromQuery] string phoneNumber)
        {
            var query = _context.Teachers
                .Include(s => s.Gender)
                .Include(x => x.MaritalStatus)
                .Include(d => d.Designation)
                .AsQueryable();

            // Apply filters

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(s => (s.FirstName + " " + (s.MiddleName ?? "") + " " + s.LastName).Trim().Contains(name));

            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(s => s.EmailAddress.Contains(email));

            if (!string.IsNullOrWhiteSpace(phoneNumber))
                query = query.Where(s => s.PhoneNumber.Contains(phoneNumber));

            var teachers = await query.ToListAsync();
            return Ok(teachers);
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PaginationModel<Teacher>>> GetPagedTeachersAsync(
            [FromQuery] int pageNumber, [FromQuery] int pageSize,
            [FromQuery] string? id, [FromQuery] string? name,
            [FromQuery] string? email, [FromQuery] string? phoneNumber,
            [FromQuery] string? sortField,
            [FromQuery] bool sortAscending = true)
        {
            var searchParameters = new SearchParameters
            {
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber,
                SortField = sortField,
                SortAscending = sortAscending
            };

            var result = await _teacherRepository.GetPagedTeachersAsync(pageNumber, pageSize, searchParameters);
            return Ok(result);
        }

        [HttpPost("Import-Teachers")]

        public async Task<IActionResult> ImportTeachers(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file provided.");
            }

            using var stream = file.OpenReadStream();
            bool result = await _teacherRepository.ImportTeachersAsync(stream);

            return result ? Ok("Teachers imported successfully.") : StatusCode(500, "Failed to import teachers.");
        }
    }
}
