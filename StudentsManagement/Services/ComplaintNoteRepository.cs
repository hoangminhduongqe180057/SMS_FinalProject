using Microsoft.EntityFrameworkCore;
using StudentsManagement.Client.Repository;
using StudentsManagement.Data;
using StudentsManagement.Shared.Models;

namespace StudentsManagement.Services
{
    public class ComplaintNoteRepository : IComplaintNoteRepository
    {
        private readonly ApplicationDbContext _context;
        public ComplaintNoteRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ComplaintNote> AddAsync(ComplaintNote complaintNote)
        {
            if (complaintNote == null) return null;
            var data = _context.ComplaintNotes.Add(complaintNote).Entity;
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<ComplaintNote> DeleteAsync(int complaintNoteId)
        {
            var data = await _context.ComplaintNotes.Where(x => x.Id == complaintNoteId).FirstOrDefaultAsync();
            if (data == null) return null;
            _context.ComplaintNotes.Remove(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<List<ComplaintNote>> GetAllAsync()
        {
            var data = await _context.ComplaintNotes
                .Include(c => c.Complaint)
                .ToListAsync();
            return data;
        }

        public async Task<ComplaintNote> GetByIdAsync(int complaintNoteId)
        {
            var data = await _context.ComplaintNotes
                .Include(c => c.Complaint)
               .FirstOrDefaultAsync(x => x.Id == complaintNoteId);
            return data;
        }

        public async Task<PaginationModel<ComplaintNote>> GetPagedComplaintNotesAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _context.ComplaintNotes.CountAsync();
            var complaints = await _context.ComplaintNotes
                .Include(c => c.Complaint)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationModel<ComplaintNote>
            {
                Items = complaints,
                TotalItems = totalItems,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<ComplaintNote> UpdateAsync(ComplaintNote complaintNote)
        {
            if (complaintNote == null) return null;
            var data = _context.ComplaintNotes.Update(complaintNote).Entity;
            await _context.SaveChangesAsync();
            return data;
        }
    }
}
