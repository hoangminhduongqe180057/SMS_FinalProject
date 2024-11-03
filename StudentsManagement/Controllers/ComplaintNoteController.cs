using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentsManagement.Data;
using StudentsManagement.Shared.Models;

namespace StudentsManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintNoteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ComplaintNoteController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("All-ComplaintNotes")]
        public async Task<ActionResult<IEnumerable<ComplaintNote>>> GetAllComplaintNotes()
        {
            return await _context.ComplaintNotes.ToListAsync();
        }

        [HttpGet("Single-ComplaintNote/{id}")]
        public async Task<ActionResult<ComplaintNote>> GetSingleComplaintNote(int id)
        {
            var complaint = await _context.ComplaintNotes.FindAsync(id);

            if (complaint == null)
            {
                return NotFound();
            }

            return complaint;
        }

        [HttpPut("Update-ComplaintNote")]
        public async Task<IActionResult> UpdateSingleComplaintNote(int id, ComplaintNote complaint)
        {
            if (id != complaint.Id)
            {
                return BadRequest();
            }

            _context.Entry(complaint).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComplaintExists(id))
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

        [HttpPost("Add-ComplaintNote")]
        public async Task<ActionResult<Complaint>> AddNewComplaintNote(ComplaintNote complaint)
        {
            _context.ComplaintNotes.Add(complaint);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComplaintNote", new { id = complaint.Id }, complaint);
        }

        [HttpDelete("Delete-ComplaintNote/{id}")]
        public async Task<IActionResult> DeleteComplaintNote(int id)
        {
            var complaint = await _context.ComplaintNotes.FindAsync(id);
            if (complaint == null)
            {
                return NotFound();
            }

            _context.ComplaintNotes.Remove(complaint);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ComplaintExists(int id)
        {
            return _context.ComplaintNotes.Any(e => e.Id == id);
        }
    }
}
