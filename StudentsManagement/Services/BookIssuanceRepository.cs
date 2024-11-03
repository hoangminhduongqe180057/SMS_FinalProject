using Microsoft.EntityFrameworkCore;
using StudentsManagement.Client.Repository;
using StudentsManagement.Data;
using StudentsManagement.Shared.Models;

namespace StudentsManagement.Services
{
    public class BookIssuanceRepository : IBookIssuanceRepository
    {
        private readonly ApplicationDbContext _context;
        public BookIssuanceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BookIssuance> AddAsync(BookIssuance bookIssuance)
        {
            var issueStatus = await _context.SystemCodeDetails
                .Include(x => x.SystemCode)
                .Where(x => x.SystemCode.Code == "BookIssuanceStatus" && x.Code == "Issued")
                .FirstOrDefaultAsync();

            bookIssuance.StatusId = issueStatus.Id;
            bookIssuance.CreatedOn = DateTime.Now;
            if (bookIssuance == null) return null;
            var data = _context.BookIssuances.Add(bookIssuance).Entity;
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<BookIssuance> DeleteAsync(int bookIssuanceId)
        {
            var data = await _context.BookIssuances.Where(x => x.Id == bookIssuanceId).FirstOrDefaultAsync();
            if (data == null) return null;
            _context.BookIssuances.Remove(data);
            await _context.SaveChangesAsync();
            return data;

        }

        public async Task<List<BookIssuance>> GetAllAsync()
        {
            var data = await _context.BookIssuances
                .Include(s => s.Student)
                .Include(c => c.Class)
                .Include(b => b.Book)
                .Include(t => t.Status)
                .ToListAsync();
            return data;
        }

        public async Task<BookIssuance> GetByIdAsync(int bookIssuanceId)
        {
            var data = await _context.BookIssuances
                .Include(s => s.Student)
                .Include(c => c.Class)
                .Include(b => b.Book)
                .Include(t => t.Status)
                .FirstOrDefaultAsync(x => x.Id == bookIssuanceId);
            return data;
        }

        public async Task<BookIssuance> UpdateAsync(BookIssuance bookIssuance)
        {
            if (bookIssuance == null) return null;
            var data = _context.BookIssuances.Update(bookIssuance).Entity;
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<BookIssuance> ReturnBookUpdateAsync(BookIssuance bookIssuance)
        {
            if (bookIssuance == null) return null;
            var returnStatus = await _context.SystemCodeDetails
                .Include(x => x.SystemCode)
                .Where(x => x.SystemCode.Code == "BookIssuanceStatus" && x.Code == "Returned")
                .FirstOrDefaultAsync();
            bookIssuance.StatusId = returnStatus.Id;
            var data = _context.BookIssuances.Update(bookIssuance).Entity;
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<PaginationModel<BookIssuance>> GetPagedBookIssuancesAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _context.BookIssuances.CountAsync();
            var bookIssuances = await _context.BookIssuances
                .Include(s => s.Student)
                .Include(c => c.Class)
                .Include(b => b.Book)
                .Include(t => t.Status)
                .Where(x => x.Status.Code == "Issued")
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationModel<BookIssuance>
            {
                Items = bookIssuances,
                TotalItems = totalItems,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<PaginationModel<BookIssuance>> GetPagedBookReturnedAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _context.BookIssuances.CountAsync();
            var bookIssuances = await _context.BookIssuances
                .Include(s => s.Student)
                .Include(c => c.Class)
                .Include(b => b.Book)
                .Include(t => t.Status)
                .Where(x => x.Status.Code == "Returned")
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationModel<BookIssuance>
            {
                Items = bookIssuances,
                TotalItems = totalItems,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
