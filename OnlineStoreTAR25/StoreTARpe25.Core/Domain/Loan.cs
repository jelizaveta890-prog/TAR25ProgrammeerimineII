public class Loan
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public Book Book { get; set; } 

    public Guid ReaderId { get; set; }
    public Reader Reader { get; set; } 

    public DateTime LoanDate { get; set; }
    public DateTime? ReturnDate { get; set; } 
}
