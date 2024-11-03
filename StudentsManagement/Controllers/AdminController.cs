using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentsManagement.Data; // Thêm namespace nếu cần
using System.Threading.Tasks;

namespace StudentsManagement.Controllers
{
    [Authorize(Roles = "System")] // Chỉ dành cho admin
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        // Inject UserManager vào constructor
        public AdminController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // API lấy danh sách tài khoản đang chờ phê duyệt
        [HttpGet("pending-accounts")]
        public async Task<IActionResult> GetPendingAccounts()
        {
            var pendingUsers = await _userManager.Users
                .Where(u => u.AccountStatus == "Pending")
                .ToListAsync();

            return Ok(pendingUsers);
        }

        // API chấp thuận tài khoản
        [HttpPost("approve-account/{userId}")]
        public async Task<IActionResult> ApproveAccount(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            user.AccountStatus = "Approved";
            await _userManager.UpdateAsync(user);

            // Gửi email thông báo cho người dùng
            // Có thể thêm logic gửi email xác nhận tài khoản đã được chấp thuận

            return Ok(new { Message = "Account approved successfully." });
        }

        // API từ chối tài khoản
        [HttpPost("reject-account/{userId}")]
        public async Task<IActionResult> RejectAccount(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            user.AccountStatus = "Rejected";
            await _userManager.UpdateAsync(user);

            // Gửi email thông báo cho người dùng
            // Có thể thêm logic gửi email thông báo tài khoản bị từ chối

            return Ok(new { Message = "Account rejected successfully." });
        }
    }
}
