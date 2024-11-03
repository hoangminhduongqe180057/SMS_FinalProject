using Microsoft.AspNetCore.Mvc;
using StudentsManagement.Client.Services;
using StudentsManagement.Client.Repository;

namespace StudentsManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly ExportService _exportService;

        public ExportController(IBookRepository bookRepository, ExportService exportService)
        {
            _bookRepository = bookRepository;
            _exportService = exportService;
        }

        // Export Books to CSV
        [HttpGet("ExportBooksToCsv")]
        public async Task<IActionResult> ExportBooksToCsv()
        {
            var books = await _bookRepository.GetAllAsync(); // Fetch data from repository
            var csvData = _exportService.ExportToCsv(books);

            return File(csvData, "text/csv", "Books.csv");
        }

        // Export Books to Excel
        [HttpGet("ExportBooksToExcel")]
        public async Task<IActionResult> ExportBooksToExcel()
        {
            var books = await _bookRepository.GetAllAsync(); // Fetch data from repository
            var excelData = _exportService.ExportToExcel(books, "Books");

            return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Books.xlsx");
        }
    }
}
