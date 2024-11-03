using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentsManagement.Data;
using StudentsManagement.Shared.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
namespace StudentsManagement.Services
{
    public class UserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UserRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // Get all users
        public async Task<List<ApplicationUser>> GetAllAsync()
        {
            var users = await _userManager.Users
    .Select(user => new ApplicationUser
    {
        Id = user.Id,
        AccountStatus = user.AccountStatus ?? "",
        Email = user.Email,
        PhoneNumber = user.PhoneNumber,  // Handling NULL phone number
        Photo = user.Photo ?? "default.jpg",      // Handling NULL photo
        DeactivatedOn = user.DeactivatedOn,       // Check if nullable handling is required here
        IsActive = user.IsActive,        // Handle nullable bool (if needed)
        LastActivityDate = user.LastActivityDate != DateTime.MinValue ? user.LastActivityDate : DateTime.Now,
        FirstName = user.FirstName,
        MiddleName = user.MiddleName ?? "",       // Handle nullable middle name
        LastName = user.LastName,
        Gender = user.Gender != null ? user.Gender : new SystemCodeDetail { Description = "N/A" }
    }).ToListAsync();

            if (users == null || !users.Any())
            {
                Console.WriteLine("No users found");
            }
            else
            {
                foreach (var user in users)
                {
                    Console.WriteLine($"User: {user.FirstName} {user.LastName}, Email: {user.Email}");
                }
            }

            return users;
        }

        // Get a user by Id
        public async Task<ApplicationUser> GetByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        // Update a user
        public async Task<IdentityResult> UpdateAsync(ApplicationUser user)
        {
            return await _userManager.UpdateAsync(user);
        }

        // Delete a user
        public async Task<IdentityResult> DeleteAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                return await _userManager.DeleteAsync(user);
            }
            return IdentityResult.Failed();
        }

        // Get paged users
        public async Task<PaginationModel<ApplicationUser>> GetPagedUsersAsync(int pageNumber, int pageSize)
        {

            try
            {
                var query = _userManager.Users.Select(user => new ApplicationUser
                {
                    Id = user.Id,
                    AccountStatus = user.AccountStatus ?? "",
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Photo = user.Photo ?? "default.jpg",
                    DeactivatedOn = user.DeactivatedOn,
                    IsActive = user.IsActive,
                    LastActivityDate = user.LastActivityDate != DateTime.MinValue ? user.LastActivityDate : DateTime.Now,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName ?? "",
                    LastName = user.LastName,
                    Gender = user.Gender != null ? user.Gender : new SystemCodeDetail { Description = "N/A" }
                });

                var totalItems = await query.CountAsync();
                var users = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (users == null)
                {
                    Console.WriteLine("No users found");
                    return null;
                }

                return new PaginationModel<ApplicationUser>
                {
                    Items = users,
                    TotalItems = totalItems,
                    CurrentPage = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                // Log the exception to the console for debugging purposes
                Console.WriteLine($"Error in GetPagedUsersAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string oldPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task<ApplicationUser?> GetCurrentUserAsync(ClaimsPrincipal userPrincipal)
        {
            return await _userManager.GetUserAsync(userPrincipal);
        }
    }
}
