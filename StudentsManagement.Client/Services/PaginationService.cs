using StudentsManagement.Client.Repository;
using StudentsManagement.Shared.Models;

namespace StudentsManagement.Client.Services
{
    public class PaginationService : IPaginationRepository
    {
        private readonly IPaginationRepository _paginationRepository;

        // Inject repository để làm việc với dữ liệu phân trang
        public PaginationService(IPaginationRepository paginationRepository)
        {
            _paginationRepository = paginationRepository;
        }

        // Lấy dữ liệu phân trang thông qua repository
        public async Task<PaginationModel<T>> GetPagedDataAsync<T>(IQueryable<T> query, int pageNumber, int pageSize) where T : class
        {
            // Gọi phương thức GetPagedDataAsync từ PaginationRepository
            return await _paginationRepository.GetPagedDataAsync(query, pageNumber, pageSize);
        }
    }
}
