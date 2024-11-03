using Microsoft.EntityFrameworkCore;
using StudentsManagement.Client.Repository;
using StudentsManagement.Data;
using StudentsManagement.Shared.Models;
using System.Net;

namespace StudentsManagement.Services
{
    public class HostelRepository : IHostelRepository
    {
        private readonly ApplicationDbContext _context;
        public HostelRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Hostel> AddAsync(Hostel Hostel)
        {
            _context.Hostels.Add(Hostel);
            await _context.SaveChangesAsync();
            return Hostel;
        }

        public async Task<Hostel> DeleteAsync(int HostelId)
        {
            var data = await _context.Hostels.Where(x => x.Id == HostelId).FirstOrDefaultAsync();
            if (data == null) return null;
            _context.Hostels.Remove(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<List<Hostel>> GetAllAsync()
        {
            var data = await _context.Hostels.Include(s => s.HostelType).ToListAsync();

            return data;
        }

        public async Task<Hostel> GetByIdAsync(int HostelId)
        {
            var data = await _context.Hostels.Include(s => s.HostelType).FirstOrDefaultAsync(s => s.Id == HostelId);
            if (data == null) return null;
            return data;
        }

        public async Task<PaginationModel<Hostel>> GetPagedHostelsAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _context.Hostels.CountAsync();
            var hotels = await _context.Hostels
                .Include(s => s.HostelType)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationModel<Hostel>
            {
                Items = hotels,
                TotalItems = totalItems,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<Hostel> UpdateAsync(Hostel Hostel)
        {
            if (Hostel == null) return null;

            var data = _context.Hostels.Update(Hostel).Entity;
            await _context.SaveChangesAsync();

            return data;
        }

        public async Task<int> GetTotalHostelsCountAsync()
        {
            return await _context.Hostels.CountAsync();
        }
    }
}
