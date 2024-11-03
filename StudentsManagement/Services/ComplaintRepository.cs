using Microsoft.EntityFrameworkCore;
using StudentsManagement.Client.Repository;
using StudentsManagement.Data;
using StudentsManagement.Shared.Models;

namespace StudentsManagement.Services
{
    public class ComplaintRepository : IComplaintRepository
    {
        private readonly ApplicationDbContext _context;
        public ComplaintRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Complaint> AddAsync(Complaint complaint)
        {
            if (complaint == null) return null;
            var data = _context.Complaints.Add(complaint).Entity;
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<Complaint> DeleteAsync(int complaintId)
        {
            var data = await _context.Complaints.Where(x => x.Id == complaintId).FirstOrDefaultAsync();
            if (data == null) return null;
            _context.Complaints.Remove(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<List<Complaint>> GetAllAsync()
        {
            var data = await _context.Complaints
                .Include(x => x.ComplaintType)
                .Include(t => t.Source)
                .Include(s => s.Status)
                .ToListAsync();
            return data;
        }

        public async Task<Complaint> GetByIdAsync(int complaintId)
        {
            var data = await _context.Complaints
                .Include(x => x.ComplaintType)
                .Include(t => t.Source)
                .Include(s => s.Status)
                .FirstOrDefaultAsync(x => x.Id == complaintId);
            return data;
        }

        public async Task<PaginationModel<Complaint>> GetPagedComplaintsAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _context.Complaints.CountAsync();
            var complaints = await _context.Complaints
                .Include(x => x.ComplaintType)
                .Include(t => t.Source)
                .Include(s => s.Status)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationModel<Complaint>
            {
                Items = complaints,
                TotalItems = totalItems,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<Complaint> UpdateAsync(Complaint complaint)
        {
            if (complaint == null) return null;
            var data = _context.Complaints.Update(complaint).Entity;
            await _context.SaveChangesAsync();
            return data;
        }
    }
}
