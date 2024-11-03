using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentsManagement.Client.Repository;
using StudentsManagement.Data;
using StudentsManagement.Shared.Models;

namespace StudentsManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IBookRepository _bookRepository;

        public BooksController(ApplicationDbContext context, IBookRepository bookRepository)
        {
            _context = context;
            _bookRepository = bookRepository;
        }

        // GET: api/Books
        [HttpGet("All-Books")]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks()
        {
            return await _context.Books.ToListAsync();
        }

        // GET: api/Books/5
        [HttpGet("Single-Book/{id}")]
        public async Task<ActionResult<Book>> GetSingleBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Update-Book")]
        public async Task<IActionResult> UpdateSingleBook(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Add-Book")]
        public async Task<ActionResult<Book>> AddNewBook(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("Delete-Book/{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }

        [HttpGet("Total-Books")]
        public async Task<ActionResult<int>> GetTotalBooksCountAsync()
        {
            var totalBooks = await _context.Books.CountAsync();
            return Ok(totalBooks);
        }

        [HttpGet("Search")]
        public async Task<ActionResult<List<Book>>> SearchBooksAsync(
            [FromQuery] string name,
            [FromQuery] string author,
            [FromQuery] string category)
        {
            var query = _context.Books
                .Include(s => s.BookCategory)
                .AsQueryable();

            // Apply filters

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(s => (s.Name).Trim().Contains(name));

            if (!string.IsNullOrWhiteSpace(author))
                query = query.Where(s => s.Auther.Contains(author));

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(s => s.BookCategory.Description.Contains(category));

            var books = await query.ToListAsync();
            return Ok(books);
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PaginationModel<Book>>> GetPagedBooksAsync(
            [FromQuery] int pageNumber, [FromQuery] int pageSize,
            [FromQuery] string? id, [FromQuery] string? name,
            [FromQuery] string? author, [FromQuery] string? noOfCopy,
            [FromQuery] string? bookCategoryId,
            [FromQuery] string? sortField,
            [FromQuery] bool sortAscending = true)
        {
            var searchParameters = new SearchParameters
            {
                Name = name,
                Author = author,
                NoOfCopy = noOfCopy,
                Category = bookCategoryId,
                SortField = sortField,
                SortAscending = sortAscending
            };

            var result = await _bookRepository.GetPagedBooksAsync(pageNumber, pageSize, searchParameters);
            return Ok(result);
        }

        [HttpPost("Import-Books")]

        public async Task<IActionResult> ImportBooks(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file provided.");
            }

            using var stream = file.OpenReadStream();
            bool result = await _bookRepository.ImportBooksAsync(stream);

            return result ? Ok("Books imported successfully.") : StatusCode(500, "Failed to import books.");
        }
    }
}
