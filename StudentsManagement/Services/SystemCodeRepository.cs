using Microsoft.EntityFrameworkCore;
using StudentsManagement.Client.Repository;
using StudentsManagement.Data;
using StudentsManagement.Shared.Models;

namespace StudentsManagement.Services
{
    public class SystemCodeRepository : ISystemCodeRepository
    {

        private readonly ApplicationDbContext _context;

        public SystemCodeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<SystemCode> AddAsync(SystemCode systemCode)
        {
            if (systemCode == null) return null;
            var newSystemCode = _context.SystemCodes.Add(systemCode).Entity;
            await _context.SaveChangesAsync();
            return newSystemCode;
        }

        public async Task<SystemCode> DeleteAsync(int systemCodeId)
        {
            var systemCode = await _context.SystemCodes.Where(x => x.Id == systemCodeId).FirstOrDefaultAsync();
            if (systemCode == null) return null;
            _context.SystemCodes.Remove(systemCode);
            await _context.SaveChangesAsync();
            return systemCode;

        }

        public async Task<List<SystemCode>> GetAllAsync()
        {
            var systemCodes = await _context.SystemCodes.ToListAsync();
            return systemCodes;
        }

        public async Task<SystemCode> GetByIdAsync(int systemCodeId)
        {
            var systemCode = await _context.SystemCodes.Where(x => x.Id == systemCodeId).FirstOrDefaultAsync();
            if (systemCode == null) return null;
            return systemCode;
        }

        public async Task<SystemCode> UpdateAsync(SystemCode systemCode)
        {
            if (systemCode == null) return null;
            var newSystemCode = _context.SystemCodes.Update(systemCode).Entity;
            await _context.SaveChangesAsync();
            return newSystemCode;
        }

        public async Task<PaginationModel<SystemCode>> GetPagedSystemCodesAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _context.SystemCodes.CountAsync();
            var systemcodes = await _context.SystemCodes
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationModel<SystemCode>
            {
                Items = systemcodes,
                TotalItems = totalItems,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
