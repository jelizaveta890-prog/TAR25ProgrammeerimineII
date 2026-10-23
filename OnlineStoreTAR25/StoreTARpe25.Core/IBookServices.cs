namespace ShopTARpe25.Core.ServiceInterface
{
    public enum DeleteBookResult { Deleted, NotFound, HasActiveLoan }

    public interface IBookServices
    {
        Task<Book> AddBookAsync(string title, string author, int publishedYear);
        Task<List<Book>> GetAllAsync();
        Task<Book?> GetByIdAsync(Guid id);
        Task<List<Book>> GetByYearAsync(int year);
        Task<Book?> GetByTitleAsync(string title);
        Task<List<Book>> GetBorrowedBooksAsync();
        Task<List<Loan>> GetLoansByReaderAsync(Guid readerId);
        Task<List<Loan>> GetLoansByBookAsync(Guid bookId);
        Task<List<Reader>> GetReadersAsync();
        Task<Reader> AddReaderAsync(string fullName, string email);
        Task<bool> UpdateTitleAsync(Guid id, string newTitle);
        Task<bool> ReturnBookAsync(Guid bookId);
        Task<DeleteBookResult> DeleteBookAsync(Guid id);
        Task<bool> LoanBookAsync(Guid bookId, Guid readerId);
    }
}