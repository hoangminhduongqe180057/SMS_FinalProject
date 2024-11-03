using Microsoft.EntityFrameworkCore;
using StudentsManagement.Client.Repository;
using StudentsManagement.Data;
using StudentsManagement.Shared.Models;

namespace StudentsManagement.Services
{
    public class AcademicYearRepository : IAcademicYearRepository
    {
        private readonly ApplicationDbContext _context;
        public AcademicYearRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AcademicYears> AddAsync(AcademicYears academicYears)
        {
            if(academicYears.EndDate < DateTime.Now)
            {
                academicYears.Status = "Inactive";
            }
            else
            {
                academicYears.Status = "Active";
            }
            _context.AcademicYears.Add(academicYears);
            await _context.SaveChangesAsync();
            return academicYears;
        }

        public async Task<AcademicYears> DeleteAsync(int academicYearsId)
        {
            var data = await _context.AcademicYears.Where(x => x.Id == academicYearsId).FirstOrDefaultAsync();
            if (data == null) return null;
            _context.AcademicYears.Remove(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<List<AcademicYears>> GetAllAsync()
        {
            var data = await _context.AcademicYears.ToListAsync();

            return data;
        }

        public async Task<AcademicYears> GetByIdAsync(int academicYearsId)
        {
            var data = await _context.AcademicYears.Where(x => x.Id == academicYearsId).FirstOrDefaultAsync();
            if (data == null) return null;
            return data;
        }

        public async Task<AcademicYears> UpdateAsync(AcademicYears academicYears)
        {
            if (academicYears == null) return null;
            if (academicYears.EndDate < DateTime.Now)
            {
                academicYears.Status = "Inactive";
            }
            else
            {
                academicYears.Status = "Active";
            }

            var data = _context.AcademicYears.Update(academicYears).Entity;
            await _context.SaveChangesAsync();

            return data;
        }

        public async Task<PaginationModel<AcademicYears>> GetPagedAcademicYearsAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _context.AcademicYears.CountAsync();
            var academicYears = await _context.AcademicYears
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationModel<AcademicYears>
            {
                Items = academicYears,
                TotalItems = totalItems,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
