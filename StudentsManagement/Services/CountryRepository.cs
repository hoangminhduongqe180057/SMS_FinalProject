using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using StudentsManagement.Client.Repository;
using StudentsManagement.Data;
using StudentsManagement.Shared.Models;

namespace StudentsManagement.Services
{
    public class CountryRepository : ICountryRepository
    {
        private readonly ApplicationDbContext _context;

        public CountryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Country> AddAsync(Country country)
        {
            if (country == null) return null;
            var newcountry = _context.Countries.Add(country).Entity;
            await _context.SaveChangesAsync();
            return newcountry;
        }

        public async Task<Country> DeleteAsync(int countryId)
        {
            var country = await _context.Countries.Where(x => x.Id == countryId).FirstOrDefaultAsync();
            if (country == null) return null;
            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();
            return country;
        }

        public async Task<List<Country>> GetAllAsync()
        {
            var countries = await _context.Countries.ToListAsync();
            return countries;
        }

        public async Task<Country> GetByIdAsync(int countryId)
        {
            var country = await _context.Countries.Where(x => x.Id == countryId).FirstOrDefaultAsync();
            if (country == null) return null;
            return country;
        }

        public async Task<Country> UpdateAsync(Country country)
        {
            if (country == null) return null;
            var newcountry = _context.Countries.Update(country).Entity;
            await _context.SaveChangesAsync();
            return newcountry;
        }

        public async Task<PaginationModel<Country>> GetPagedCountriesAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _context.Countries.CountAsync();
            var countries = await _context.Countries
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationModel<Country>
            {
                Items = countries,
                TotalItems = totalItems,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
