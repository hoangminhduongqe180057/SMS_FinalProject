using Microsoft.AspNetCore.Mvc;
using StudentsManagement.Data;
using StudentsManagement.Services;
using StudentsManagement.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentsManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<PaginationModel<ApplicationUser>>> GetAllUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var pagedUsers = await _userRepository.GetPagedUsersAsync(pageNumber, pageSize);
                if (pagedUsers == null)
                {
                    return NotFound("No users found");
                }
                return Ok(pagedUsers);
            }
            catch (Exception ex)
            {
                // Log error to the console for now, in real application, use logging framework
                Console.WriteLine($"Error in GetAllUsers: {ex.Message}");
                return StatusCode(500, "An internal server error occurred");
            }
        }
        // GET: api/user/{id}
        //[HttpGet("{id}")]
        //public async Task<ActionResult<ApplicationUser>> GetUserById(string id)
        //{
        //    var user = await _userRepository.GetByIdAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(user);
        //}

        // PUT: api/user/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, ApplicationUser user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            var result = await _userRepository.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return StatusCode(500, "An error occurred while updating the user.");
            }

            return NoContent();
        }

        // DELETE: api/user/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userRepository.DeleteAsync(id);
            if (!result.Succeeded)
            {
                return StatusCode(500, "An error occurred while deleting the user.");
            }

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUserDto>> GetUserById(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Create a DTO to send only necessary information to the client
            var userDto = new ApplicationUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };

            return Ok(userDto);
        }

        // DTO to return only the fields you need
        public class ApplicationUserDto
        {
            public string Id { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
        }
    }
}

