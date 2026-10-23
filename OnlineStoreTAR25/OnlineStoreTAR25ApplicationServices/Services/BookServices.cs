using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;

namespace ShopTARpe25.ApplicationServices.Services
{
    public class BookServices : IBookServices
    {
        private readonly ShopTARpe25Context _context;

        public BookServices(ShopTARpe25Context context) { _context = context; }

        // Ülesanne 3: lisamine (IsAvailable alati true)
        public async Task<Book> AddBookAsync(string title, string author, int publishedYear)
        {
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = title.Trim(),
                Author = author.Trim(),
                PublishedYear = publishedYear,
                IsAvailable = true
            };
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        // Ülesanne 4: LINQ päringud
        public Task<List<Book>> GetAllAsync() =>
            _context.Books.AsNoTracking().OrderBy(b => b.Title).ToListAsync();

        public Task<Book?> GetByIdAsync(Guid id) =>
            _context.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);

        public Task<List<Book>> GetByYearAsync(int year) =>
            _context.Books.AsNoTracking().Where(b => b.PublishedYear == year)
                .OrderBy(b => b.Title).ToListAsync();

        public Task<Book?> GetByTitleAsync(string title) =>
            _context.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Title == title);

        public Task<List<Book>> GetBorrowedBooksAsync() =>
            _context.Books.AsNoTracking().Where(b => !b.IsAvailable)
                .OrderBy(b => b.Title).ToListAsync();

        public Task<List<Loan>> GetLoansByReaderAsync(Guid readerId) =>
            _context.Loans.AsNoTracking()
                .Include(l => l.Book).Include(l => l.Reader)
                .Where(l => l.ReaderId == readerId)
                .OrderByDescending(l => l.LoanDate).ToListAsync();

        public Task<List<Loan>> GetLoansByBookAsync(Guid bookId) =>
            _context.Loans.AsNoTracking()
                .Include(l => l.Reader)
                .Where(l => l.BookId == bookId)
                .OrderByDescending(l => l.LoanDate).ToListAsync();

        public Task<List<Reader>> GetReadersAsync() =>
            _context.Readers.AsNoTracking().OrderBy(r => r.FullName).ToListAsync();

        // Ülesanne 5: lugejate lisamine
        public async Task<Reader> AddReaderAsync(string fullName, string email)
        {
            var reader = new Reader
            {
                Id = Guid.NewGuid(),
                FullName = fullName.Trim(),
                Email = email.Trim()
            };
            _context.Readers.Add(reader);
            await _context.SaveChangesAsync();
            return reader;
        }

        // Ülesanne 5: muutmine ja kustutamine
        public async Task<bool> UpdateTitleAsync(Guid id, string newTitle)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null) return false;
            book.Title = newTitle.Trim();
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReturnBookAsync(Guid bookId)
        {
            var loan = await _context.Loans.Include(l => l.Book)
                .FirstOrDefaultAsync(l => l.BookId == bookId && l.ReturnDate == null);
            if (loan == null) return false;

            loan.ReturnDate = DateTime.Now;
            loan.Book.IsAvailable = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<DeleteBookResult> DeleteBookAsync(Guid id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null) return DeleteBookResult.NotFound;

            // Kontroll: laenutatud raamatut ei kustutata
            bool hasActiveLoan = !book.IsAvailable ||
                await _context.Loans.AnyAsync(l => l.BookId == id && l.ReturnDate == null);
            if (hasActiveLoan) return DeleteBookResult.HasActiveLoan;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return DeleteBookResult.Deleted;
        }

        public async Task<bool> LoanBookAsync(Guid bookId, Guid readerId)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == bookId);
            var readerExists = await _context.Readers.AnyAsync(r => r.Id == readerId);
            if (book == null || !readerExists || !book.IsAvailable) return false;

            _context.Loans.Add(new Loan
            {
                Id = Guid.NewGuid(),
                BookId = bookId,
                ReaderId = readerId,
                LoanDate = DateTime.Now,
                ReturnDate = null
            });
            book.IsAvailable = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}