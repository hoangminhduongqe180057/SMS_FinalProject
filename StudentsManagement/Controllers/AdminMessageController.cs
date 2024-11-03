using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentsManagement.Data;
using StudentsManagement.Shared.Models;

namespace StudentsManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminMessagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminMessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<AdminMessage>>> GetMessages()
        {
            return await _context.AdminMessages.OrderBy(m => m.SentAt).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> PostMessage(AdminMessage message)
        {
            try
            {
                message.SentAt = DateTime.Now;
                _context.AdminMessages.Add(message);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                // Log lỗi ra file hoặc console để debug
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
