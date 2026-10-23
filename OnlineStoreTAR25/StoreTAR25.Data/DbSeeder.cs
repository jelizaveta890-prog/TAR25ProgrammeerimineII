using ShopTARpe25.Data;

namespace ShopTARpe25.Data
{
    public static class DbSeeder
    {
        public static void Seed(ShopTARpe25Context db)
        {
            if (db.Readers.Any()) return;

            db.Readers.AddRange(
                new Reader { Id = Guid.NewGuid(), FullName = "Mari Maasikas", Email = "mari@example.com" },
                new Reader { Id = Guid.NewGuid(), FullName = "Jaan Tamm", Email = "jaan@example.com" }
            );
            db.SaveChanges();
        }
    }
}