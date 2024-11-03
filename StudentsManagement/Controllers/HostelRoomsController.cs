using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentsManagement.Data;
using StudentsManagement.Migrations;
using StudentsManagement.Shared.Models;

namespace StudentsManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HostelRoomsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HostelRoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("All-HostelRooms")]
        public async Task<ActionResult<IEnumerable<HostelRoom>>> GetAllHostelRooms()
        {
            return await _context.HostelRooms.ToListAsync();
        }

        [HttpGet("Single-HostelRoom/{id}")]
        public async Task<ActionResult<HostelRoom>> GetSingleHostelRoom(int id)
        {
            var hostelroom = await _context.HostelRooms.FindAsync(id);

            if (hostelroom == null)
            {
                return NotFound();
            }

            return hostelroom;
        }

        [HttpPut("Update-Hostel")]
        public async Task<IActionResult> UpdateSingleHostelRoom(int id, HostelRoom hostel)
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
                if (!HostelRoomExists(id))
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

        [HttpPost("Add-HostelRoom")]
        public async Task<ActionResult<Book>> AddNewBook(HostelRoom hostel)
        {
            _context.HostelRooms.Add(hostel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = hostel.Id }, hostel);
        }

        [HttpDelete("Delete-HostelRoom/{id}")]
        public async Task<IActionResult> DeleteHostelRoom(int id)
        {
            var hostel = await _context.HostelRooms.FindAsync(id);
            if (hostel == null)
            {
                return NotFound();
            }

            _context.HostelRooms.Remove(hostel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HostelRoomExists(int id)
        {
            return _context.HostelRooms.Any(e => e.Id == id);
        }

        [HttpGet("Total-HostelRooms")]
        public async Task<ActionResult<int>> GetTotalHostelRoomsCountAsync()
        {
            var total = await _context.HostelRooms.CountAsync();
            return Ok(total);
        }
    }
}
