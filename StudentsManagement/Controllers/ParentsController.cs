using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentsManagement.Client.Repository;
using StudentsManagement.Data;
using StudentsManagement.Shared.Models;

namespace StudentsManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IParentRepository _parentRepository;

        public ParentsController(ApplicationDbContext context, IParentRepository parentRepository)
        {
            _context = context;
            _parentRepository = parentRepository;
        }

        // GET: api/Parents
        [HttpGet("All-Parents")]
        public async Task<ActionResult<IEnumerable<Parent>>> GetAllParents()
        {
            return await _context.Parents.ToListAsync();
        }

        // GET: api/Parents/5
        [HttpGet("Single-Parent/{id}")]
        public async Task<ActionResult<Parent>> GetSingleParent(int id)
        {
            var parent = await _context.Parents.FindAsync(id);

            if (parent == null)
            {
                return NotFound();
            }

            return parent;
        }

        // PUT: api/Parents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Update-Parent")]
        public async Task<IActionResult> UpdateSingleParent(int id, Parent parent)
        {
            if (id != parent.Id)
            {
                return BadRequest();
            }

            _context.Entry(parent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParentExists(id))
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

        // POST: api/Parents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Add-Parent")]
        public async Task<ActionResult<Parent>> AddNewParent(Parent parent)
        {
            _context.Parents.Add(parent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetParent", new { id = parent.Id }, parent);
        }

        // DELETE: api/Parents/5
        [HttpDelete("Delete-Parent/{id}")]
        public async Task<IActionResult> DeleteParent(int id)
        {
            var parent = await _context.Parents.FindAsync(id);
            if (parent == null)
            {
                return NotFound();
            }

            _context.Parents.Remove(parent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ParentExists(int id)
        {
            return _context.Parents.Any(e => e.Id == id);
        }

        [HttpGet("Search")]
        public async Task<ActionResult<List<Parent>>> SearchParentsAsync(
            [FromQuery] string name,
            [FromQuery] string email,
            [FromQuery] string phoneNumber,
            [FromQuery] string studentName,
            [FromQuery] string classes)
        {
            var query = _context.Parents
                .Include(x => x.Student)
                .Include(s => s.Gender)
                .Include(p => p.ParentType)
                .Include(c => c.Class)
                .AsQueryable();

            // Apply filters

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(s => (s.FirstName + " " + (s.MiddleName ?? "") + " " + s.LastName).Trim().Contains(name));

            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(s => s.EmailAddress.Contains(email));

            if (!string.IsNullOrWhiteSpace(phoneNumber))
                query = query.Where(s => s.PhoneNumber.Contains(phoneNumber));

            if (!string.IsNullOrWhiteSpace(studentName))
            {
                query = query.Where(s =>
                    (s.Student.FirstName + " " + s.Student.LastName)
                    .Trim().Contains(studentName));
            }

            if (!string.IsNullOrWhiteSpace(classes))
                query = query.Where(s => s.Class.Description.Contains(classes));

            var teachers = await query.ToListAsync();
            return Ok(teachers);
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PaginationModel<Teacher>>> GetPagedTeachersAsync(
            [FromQuery] int pageNumber, [FromQuery] int pageSize,
            [FromQuery] string? id, [FromQuery] string? name,
            [FromQuery] string? email, [FromQuery] string? phoneNumber,
            [FromQuery] string? studentName, [FromQuery] string? classes,
            [FromQuery] string? sortField,
            [FromQuery] bool sortAscending = true)
        {
            var searchParameters = new SearchParameters
            {
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber,
                StudentName = studentName,
                Class = classes,
                SortField = sortField,
                SortAscending = sortAscending
            };

            var result = await _parentRepository.GetPagedParentsAsync(pageNumber, pageSize, searchParameters);
            return Ok(result);
        }
    }
}
