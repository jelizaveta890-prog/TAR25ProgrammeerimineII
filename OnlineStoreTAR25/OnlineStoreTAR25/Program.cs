using Microsoft.EntityFrameworkCore;
using ShopTARpe25.ApplicationServices.Services;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;

namespace OnlineStoreTAR25
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            // dependency injection: teenused, mida kontrollerid kasutavad
            builder.Services.AddScoped<ILibraryServices, LibraryServices>();
            builder.Services.AddScoped<IFileServices, FileServices>();
            builder.Services.AddScoped<IBookServices, BookServices>();

            // Ühenduse seadistus: connection string on appsettings.json failis
            // ("ConnectionStrings:DefaultConnection"), siin antakse see DbContext'ile (SQL Server / LocalDB).
            builder.Services.AddDbContext<ShopTARpe25Context>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            // Loob tabelid (kui neid pole) ja lisab näidisandmed, et esileht poleks tühi
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ShopTARpe25Context>();
                db.Database.Migrate();
                DbSeeder.Seed(db);
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}