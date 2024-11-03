using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentsManagement.Data;
using StudentsManagement.Shared.Models;

namespace StudentsManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HostelsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HostelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("All-Hostels")]
        public async Task<ActionResult<IEnumerable<Hostel>>> GetAllHostels()
        {
            return await _context.Hostels.ToListAsync();
        }

        [HttpGet("Single-Hostel/{id}")]
        public async Task<ActionResult<Hostel>> GetSingleHostel(int id)
        {
            var hostel = await _context.Hostels.FindAsync(id);

            if (hostel == null)
            {
                return NotFound();
            }

            return hostel;
        }

        [HttpPut("Update-Hostel")]
        public async Task<IActionResult> UpdateSingleHostel(int id, Hostel hostel)
        {
            if (id != hostel.Id)
            {
                return BadRequest();
            }

            _context.Entry(hostel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HostelExists(id))
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

        [HttpPost("Add-Hostel")]
        public async Task<ActionResult<Book>> AddNewBook(Hostel hostel)
        {
            _context.Hostels.Add(hostel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = hostel.Id }, hostel);
        }

        [HttpDelete("Delete-Hostel/{id}")]
        public async Task<IActionResult> DeleteHostel(int id)
        {
            var hostel = await _context.Hostels.FindAsync(id);
            if (hostel == null)
            {
                return NotFound();
            }

            _context.Hostels.Remove(hostel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HostelExists(int id)
        {
            return _context.Hostels.Any(e => e.Id == id);
        }

        [HttpGet("Total-Hostels")]
        public async Task<ActionResult<int>> GetTotalHostelsCountAsync()
        {
            var total = await _context.Hostels.CountAsync();
            return Ok(total);
        }
    }
}
