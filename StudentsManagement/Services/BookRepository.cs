using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using StudentsManagement.Client.Repository;
using StudentsManagement.Data;
using StudentsManagement.Shared.Models;
using System.Text;

namespace StudentsManagement.Services
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;
        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Book> AddAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return book;
        }

        public async Task<Book> DeleteAsync(int bookId)
        {
            var book = await _context.Books.Where(x => x.Id == bookId).FirstOrDefaultAsync();
            if (book == null) return null;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return book;
        }

        public async Task<List<Book>> GetAllAsync()
        {
            var books = await _context.Books.Include(s => s.BookCategory).ToListAsync();

            return books;
        }

        public async Task<Book> GetByIdAsync(int bookId)
        {
            var book = await _context.Books.Include(s => s.BookCategory).FirstOrDefaultAsync(s => s.Id == bookId);

            return book;
        }

        public async Task<Book> UpdateAsync(Book book)
        {
            if (book == null) return null;


            var newbook = _context.Books.Update(book).Entity;
            await _context.SaveChangesAsync();

            return newbook;
        }

        public async Task<PaginationModel<Book>> GetPagedBooksAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _context.Books.CountAsync();
            var books = await _context.Books
                .Include(s => s.BookCategory)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationModel<Book>
            {
                Items = books,
                TotalItems = totalItems,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<int> GetTotalBooksCountAsync()
        {
            return await _context.Books.CountAsync();
        }

        public async Task<byte[]> ExportBooksToCsvAsync()
        {
            var books = await GetAllAsync();
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Id,Name,Author,NoOfCopy,Categories");
            foreach (var book in books)
            {
                csvBuilder.AppendLine($"{book.Id},{book.Name},{book.Auther},{book.NoOfCopy},{book.BookCategory?.Description}");
            }
            return Encoding.UTF8.GetBytes(csvBuilder.ToString());
        }

        public async Task<PaginationModel<Book>> GetPagedBooksAsync(int pageNumber, int pageSize, SearchParameters searchParameters = null)
        {
            var query = _context.Books
                .Include(s => s.BookCategory)
                .OrderBy(s => s.Id)
                .AsQueryable();

            // Apply search filters if provided
            if (searchParameters != null)
            {
                if (!string.IsNullOrEmpty(searchParameters.Name))
                {
                    string normalizedSearchName = searchParameters.Name.Trim();
                    query = query.Where(s => (s.Name).Trim().Contains(searchParameters.Name));
                }

                if (!string.IsNullOrEmpty(searchParameters.Author))
                    query = query.Where(s => s.Auther.Contains(searchParameters.Author));

                if (!string.IsNullOrEmpty(searchParameters.Category))
                    query = query.Where(s => s.BookCategory.Description.Contains(searchParameters.Category));
            }

            // Áp dụng sắp xếp
            if (!string.IsNullOrEmpty(searchParameters.SortField))
            {
                switch (searchParameters.SortField)
                {
                    case "Name":
                        query = searchParameters.SortAscending
                            ? query.OrderBy(s => s.Name)
                            : query.OrderByDescending(s => s.Name);
                        break;
                    case "Author":
                        query = searchParameters.SortAscending
                            ? query.OrderBy(s => s.Auther)
                            : query.OrderByDescending(s => s.Auther);
                        break;
                    case "NoOfCopy":
                        query = searchParameters.SortAscending
                            ? query.OrderBy(s => s.NoOfCopy)
                            : query.OrderByDescending(s => s.NoOfCopy);
                        break;
                    case "BookCategory":
                        query = searchParameters.SortAscending
                            ? query.OrderBy(s => s.BookCategory.Description)
                            : query.OrderByDescending(s => s.BookCategory.Description);
                        break;
                    default:
                        query = searchParameters.SortAscending
                            ? query.OrderBy(s => s.Id)
                            : query.OrderByDescending(s => s.Id);
                        break;
                }
            }

            // Pagination
            var totalItems = await query.CountAsync();
            var books = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationModel<Book>
            {
                Items = books,
                TotalItems = totalItems,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<bool> ImportBooksAsync(Stream fileStream)
        {
            var books = new List<Book>();

            using var reader = new StreamReader(fileStream, Encoding.UTF8, leaveOpen: true);

            string line;
            // Optionally skip header row
            await reader.ReadLineAsync();

            while ((line = await reader.ReadLineAsync()) != null)
            {
                var values = line.Split(',');

                if (values.Length >= 4) // Ensure enough fields are present
                {
                    try
                    {
                        // Try to parse necessary fields to ensure correct data type conversion
                        if (int.TryParse(values[3], out var bookCategoryId))
                        {
                            var book = new Book
                            {
                                Name = values[0], // Book name
                                Auther = values[1], // Book author
                                NoOfCopy = values[2], // No of copies
                                BookCateGoryId = bookCategoryId // Category ID
                            };
                            books.Add(book);
                        }
                        else
                        {
                            Console.WriteLine("Failed to parse category data for book.");
                        }
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"Format error on line: {ex.Message}");
                        continue;  // Skip invalid lines and log error
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing line: {ex.Message}");
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("Skipping line due to insufficient fields.");
                }
            }

            if (books.Any())
            {
                _context.Books.AddRange(books);
                await _context.SaveChangesAsync();  // Save all imported records asynchronously
                Console.WriteLine($"{books.Count} books imported successfully.");
                return true;
            }

            Console.WriteLine("No valid book to import.");
            return false;
        }

    }
}

