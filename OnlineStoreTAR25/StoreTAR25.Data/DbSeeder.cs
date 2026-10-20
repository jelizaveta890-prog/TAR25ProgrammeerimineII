namespace ShopTARpe25.Data
{
    public static class DbSeeder
    {
        public static void Seed(ShopTARpe25Context db)
        {
            if (db.Books.Any()) return;

            var b1 = new Book { Id = Guid.NewGuid(), Title = "Kevade", Author = "Oskar Luts", PublishedYear = 1912, IsAvailable = true };
            var b2 = new Book { Id = Guid.NewGuid(), Title = "Tõde ja õigus", Author = "A. H. Tammsaare", PublishedYear = 1926, IsAvailable = true };
            var b3 = new Book { Id = Guid.NewGuid(), Title = "Mees, kes teadis ussisõnu", Author = "Andrus Kivirähk", PublishedYear = 2007, IsAvailable = false };
            var b4 = new Book { Id = Guid.NewGuid(), Title = "Rehepapp", Author = "Andrus Kivirähk", PublishedYear = 2000, IsAvailable = true };
            var b5 = new Book { Id = Guid.NewGuid(), Title = "Clean Code", Author = "Robert C. Martin", PublishedYear = 2010, IsAvailable = false };
            var b6 = new Book { Id = Guid.NewGuid(), Title = "Harry Potter ja tarkade kivi", Author = "J. K. Rowling", PublishedYear = 2000, IsAvailable = true };

            var r1 = new Reader { Id = Guid.NewGuid(), FullName = "Mari Maasikas", Email = "mari@example.com" };
            var r2 = new Reader { Id = Guid.NewGuid(), FullName = "Jaan Tamm", Email = "jaan@example.com" };

            db.Books.AddRange(b1, b2, b3, b4, b5, b6);
            db.Readers.AddRange(r1, r2);
            db.Loans.AddRange(
                new Loan { Id = Guid.NewGuid(), BookId = b3.Id, ReaderId = r1.Id, LoanDate = DateTime.Now.AddDays(-5), ReturnDate = null },
                new Loan { Id = Guid.NewGuid(), BookId = b5.Id, ReaderId = r2.Id, LoanDate = DateTime.Now.AddDays(-2), ReturnDate = null },
                new Loan { Id = Guid.NewGuid(), BookId = b1.Id, ReaderId = r1.Id, LoanDate = DateTime.Now.AddDays(-30), ReturnDate = DateTime.Now.AddDays(-15) }
            );
            db.SaveChanges();
        }
    }
}