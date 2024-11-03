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
    public class SubjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ISubjectRepository _subjectRepository;

        public SubjectsController(ApplicationDbContext context, ISubjectRepository subjectRepository)
        {
            _context = context;
            _subjectRepository = subjectRepository;
        }

        // GET: api/Subjects
        [HttpGet("All-Subjects")]
        public async Task<ActionResult<IEnumerable<Subject>>> GetAllSubjects()
        {
            return await _context.Subjects.ToListAsync();
        }

        // GET: api/Subjects/5
        [HttpGet("Single-Subject/{id}")]
        public async Task<ActionResult<Subject>> GetSingleSubject(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);

            if (subject == null)
            {
                return NotFound();
            }

            return subject;
        }

        // PUT: api/Subjects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Update-Subject")]
        public async Task<IActionResult> UpdateSingleSubject(int id, Subject subject)
        {
            if (id != subject.Id)
            {
                return BadRequest();
            }

            _context.Entry(subject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectExists(id))
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

        // POST: api/Subjects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Add-Subject")]
        public async Task<ActionResult<Subject>> AddNewSubject(Subject subject)
        {
            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubject", new { id = subject.Id }, subject);
        }

        // DELETE: api/Subjects/5
        [HttpDelete("Delete-Subject/{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }

            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubjectExists(int id)
        {
            return _context.Subjects.Any(e => e.Id == id);
        }

        [HttpGet("Search")]
        public async Task<ActionResult<List<Subject>>> SearchSubjectsAsync(
            [FromQuery] string name)
        {
            var query = _context.Subjects.AsQueryable();

            // Apply filters

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(s => (s.Name).Trim().Contains(name));

            var subjects = await query.ToListAsync();
            return Ok(subjects);
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PaginationModel<Subject>>> GetPagedSubjectsAsync(
            [FromQuery] int pageNumber, [FromQuery] int pageSize,
            [FromQuery] string? id, [FromQuery] string? name,
            [FromQuery] string? description, [FromQuery] string? createBy,
            [FromQuery] DateTime createOn,
            [FromQuery] string? sortField,
            [FromQuery] bool sortAscending = true)
        {
            var searchParameters = new SearchParameters
            {
                Name = name,
                Description = description,
                CreateBy = createBy,
                CreateOn = createOn,
                SortField = sortField,
                SortAscending = sortAscending
            };

            var result = await _subjectRepository.GetPagedSubjectsAsync(pageNumber, pageSize, searchParameters);
            return Ok(result);
        }
    }
}
