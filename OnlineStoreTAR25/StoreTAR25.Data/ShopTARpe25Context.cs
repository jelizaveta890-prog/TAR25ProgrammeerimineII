using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;

namespace ShopTARpe25.Data
{
    // DbContext = sild C# objektide ja andmebaasi vahel.
    // Ühenduse seadistus:
    //  1) connection string on appsettings.json -> "ConnectionStrings:DefaultConnection"
    //  2) Program.cs: builder.Services.AddDbContext<ShopTARpe25Context>(options =>
    //         options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    //  3) DbContext jõuab teenusesse konstruktori (dependency injection) kaudu.
    public class ShopTARpe25Context : DbContext
    {
        public ShopTARpe25Context(DbContextOptions<ShopTARpe25Context> options)
            : base(options)
        {
        }

        public DbSet<Library> Library { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Loan> Loans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seosed: raamat 1..* laenutust, lugeja 1..* laenutust
            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Book).WithMany(b => b.Loans)
                .HasForeignKey(l => l.BookId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Reader).WithMany(r => r.Loans)
                .HasForeignKey(l => l.ReaderId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}