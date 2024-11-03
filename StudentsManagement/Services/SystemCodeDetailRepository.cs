using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentsManagement.Client.Repository;
using StudentsManagement.Data;
using StudentsManagement.Migrations;
using StudentsManagement.Shared.Models;
using System.Linq;

namespace StudentsManagement.Services
{
    public class SystemCodeDetailRepository : ISystemCodeDetailRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SystemCodeDetailRepository> _logger;
        public SystemCodeDetailRepository(ApplicationDbContext context, ILogger<SystemCodeDetailRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<SystemCodeDetail> AddAsync(SystemCodeDetail scd)
        {
            if (scd == null) return null;
            var newscd = _context.SystemCodeDetails.Add(scd).Entity;
            await _context.SaveChangesAsync();
            return newscd;
        }

        public async Task<SystemCodeDetail> DeleteAsync(int scdId)
        {
            var scd = await _context.SystemCodeDetails.Where(x => x.Id == scdId).FirstOrDefaultAsync();
            if (scd == null) return null;
            _context.SystemCodeDetails.Remove(scd);
            await _context.SaveChangesAsync();
            return scd;

        }

        public async Task<List<SystemCodeDetail>> GetAllAsync()
        {
            var scds = await _context.SystemCodeDetails.Include(x => x.SystemCode).ToListAsync();
            return scds;
        }

        public async Task<List<SystemCodeDetail>> GetByCodeAsync(string code)
        {
            var scds = await _context.SystemCodeDetails
                                      .Include(x => x.SystemCode)
                                      .Where(x => x.SystemCode.Code == code)
                                      .ToListAsync();
            return scds;
        }


        public async Task<SystemCodeDetail> GetByIdAsync(int scdId)
        {
            var scd = await _context.SystemCodeDetails.Include(x => x.SystemCode).Where(x => x.Id == scdId).FirstOrDefaultAsync();
            if (scd == null) return null;
            return scd;
        }

        public async Task<SystemCodeDetail> UpdateAsync(SystemCodeDetail scd)
        {
            if (scd == null) return null;
            var newscd = _context.SystemCodeDetails.Update(scd).Entity;
            await _context.SaveChangesAsync();
            return newscd;
        }

        public async Task EnsureRoleExistsFromSystemCode(RoleManager<ApplicationRole> roleManager, int systemCodeDetailId)
        {
            try
            {
                // Get the role code and description from SystemCodeDetail
                var roleFromSystemCode = await _context.SystemCodeDetails
                                             .Where(s => s.Id == systemCodeDetailId)
                                             .Select(s => new { s.Code, s.Description })
                                             .FirstOrDefaultAsync();

                if (roleFromSystemCode == null)
                {
                    _logger.LogWarning($"No SystemCodeDetail found with Id {systemCodeDetailId}.");
                    throw new Exception($"No SystemCodeDetail found with Id {systemCodeDetailId}");
                }

                var role = await roleManager.FindByNameAsync(roleFromSystemCode.Code);

                if (role == null)
                {
                    var newRole = new ApplicationRole
                    {
                        Name = roleFromSystemCode.Code,
                        NormalizedName = roleFromSystemCode.Code.ToUpper(),
                        Description = roleFromSystemCode.Description ?? $"Default description for {roleFromSystemCode.Code}",
                        CreatedOn = DateTime.Now,
                        CreatedById = "747a796a-a960-43c5-a42a-3641c7782699", 
                        ReviewedById = "747a796a-a960-43c5-a42a-3641c7782699", 
                        ConcurrencyStamp = Guid.NewGuid().ToString()
                    };

                    var result = await roleManager.CreateAsync(newRole);

                    if (!result.Succeeded)
                    {
                        _logger.LogError($"Error creating role {newRole.Name}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        throw new Exception($"Error creating role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }

                    _logger.LogInformation($"Role {newRole.Name} successfully created and stored in AspNetRoles table.");
                }
                else
                {
                    _logger.LogInformation($"Role {role.Name} already exists in the AspNetRoles table.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in EnsureRoleExistsFromSystemCode: {ex.Message}");
                throw;
            }
        }

        public async Task<PaginationModel<SystemCodeDetail>> GetPagedSystemCodeDetailsAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _context.SystemCodeDetails.CountAsync();
            var systemcodedetails = await _context.SystemCodeDetails
                .Include(x => x.SystemCode)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationModel<SystemCodeDetail>
            {
                Items = systemcodedetails,
                TotalItems = totalItems,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task <SystemCodeDetail> GetByStatusCodeAsync(string code, string statusCode)
        {
            // Truy vấn tất cả các SystemCodeDetail có SystemCode.Code trùng khớp với tham số code
            var scds = await _context.SystemCodeDetails
                                      .Include(x => x.SystemCode)
                                      .Where(x => x.SystemCode.Code == code && x.Code == statusCode)
                                      .ToListAsync();
            return scds.FirstOrDefault();
        }

    }
}



        

