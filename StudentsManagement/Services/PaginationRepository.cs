using Microsoft.EntityFrameworkCore;
using StudentsManagement.Client.Repository;
using StudentsManagement.Shared.Models;

namespace StudentsManagement.Services
{
    public class PaginationRepository : IPaginationRepository
    {
        public async Task<PaginationModel<T>> GetPagedDataAsync<T>(IQueryable<T> query, int pageNumber, int pageSize) where T : class
        {
            // Ensure valid page size and number
            if (pageNumber < 1)
                pageNumber = 1;
            if (pageSize < 1)
                pageSize = 10; // Default page size if an invalid one is provided

            var totalItems = await query.CountAsync(); // Count total items

            // Ensure the page number doesn't exceed the total pages
            if (pageNumber > (int)Math.Ceiling((double)totalItems / pageSize))
                pageNumber = (int)Math.Ceiling((double)totalItems / pageSize);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(); // Get paged data

            return new PaginationModel<T>
            {
                Items = items,
                TotalItems = totalItems,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
