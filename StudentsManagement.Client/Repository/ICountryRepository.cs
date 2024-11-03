using StudentsManagement.Shared.Models;

namespace StudentsManagement.Client.Repository
{
    public interface ICountryRepository
    {
        Task<Country> AddAsync(Country country);
        Task<Country> UpdateAsync(Country country);
        Task<Country> DeleteAsync(int countryId);
        Task<Country> GetByIdAsync(int countryId);
        Task<List<Country>> GetAllAsync();
        Task<PaginationModel<Country>> GetPagedCountriesAsync(int pageNumber, int pageSize);
    }
}
