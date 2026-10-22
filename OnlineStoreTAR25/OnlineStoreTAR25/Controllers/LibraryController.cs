using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Data;
using ShopTARpe25.Models.Library;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShopTARpe25.Controllers
{
    public class LibraryController : Controller
    {
        private readonly ShopTARpe25Context _context;

        public LibraryController(ShopTARpe25Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(string searchTitle, int? searchYear, string filterStatus)
        {
            var booksQuery = _context.Books.AsQueryable();

            if (!string.IsNullOrEmpty(searchTitle))
            {
                booksQuery = booksQuery.Where(x => x.Title.Contains(searchTitle));
            }
            if (searchYear.HasValue)
            {
                booksQuery = booksQuery.Where(x => x.PublishedYear == searchYear.Value);
            }
            if (!string.IsNullOrEmpty(filterStatus))
            {
                if (filterStatus == "Laenutatud")
                {
                    booksQuery = booksQuery.Where(x => !x.IsAvailable);
                }
                else if (filterStatus == "Saadaval")
                {
                    booksQuery = booksQuery.Where(x => x.IsAvailable);
                }
            }

            var result = booksQuery
                .Select(x => new LibraryIndexViewModel
                {
                    Id = x.Id,
                    Name = x.Title ?? "Nimetu raamat",
                    Classification = x.Author ?? "Teadmata autor",
                    BuiltDate = new DateTime(x.PublishedYear > 0 ? x.PublishedYear : 2026, 1, 1),
                    Crew = x.IsAvailable ? 1 : 0,
                    EnginePower = x.PublishedYear
                }).ToList();

            if (result.Count == 0)
            {
                result.Add(new LibraryIndexViewModel
                {
                    Id = Guid.Empty,
                    Name = "Kevade (Andmebaas on veel tühi - lisa uus raamat!)",
                    Classification = "Oskar Luts",
                    BuiltDate = new DateTime(1912, 1, 1),
                    Crew = 1,
                    EnginePower = 1912
                });
            }

            ViewData["CurrentTitle"] = searchTitle;
            ViewData["CurrentYear"] = searchYear;
            ViewData["CurrentStatus"] = filterStatus;

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            LibraryCreateUpdateViewModel vm = new();
            return View("CreateUpdate", vm);
        }

        // Ülesanne 3: Andmete lisamine – salvestatakse täpselt need andmed, mille kasutaja sisestas
        [HttpPost]
        public async Task<IActionResult> Create(LibraryCreateUpdateViewModel vm)
        {
            ModelState.Clear();

            // Loeme andmed otse vormilt, kui mudeli sidumine need kätte ei saanud
            string name = !string.IsNullOrWhiteSpace(vm.Name) ? vm.Name : Request.Form["Name"].ToString();
            string author = !string.IsNullOrWhiteSpace(vm.Classification) ? vm.Classification : Request.Form["Classification"].ToString();
            DateTime? date = vm.BuiltDate;
            if (!date.HasValue && DateTime.TryParse(Request.Form["BuiltDate"], out var parsed))
                date = parsed;

            vm.Name = name;
            vm.Classification = author;
            vm.BuiltDate = date;

            if (string.IsNullOrWhiteSpace(name))
                ModelState.AddModelError(nameof(vm.Name), "Pealkiri on kohustuslik.");
            if (string.IsNullOrWhiteSpace(author))
                ModelState.AddModelError(nameof(vm.Classification), "Autor on kohustuslik.");
            if (!date.HasValue)
                ModelState.AddModelError(nameof(vm.BuiltDate), "Ilmumiskuupäev on kohustuslik.");

            if (!ModelState.IsValid)
                return View("CreateUpdate", vm);

            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = name.Trim(),
                Author = author.Trim(),
                PublishedYear = date!.Value.Year,
                IsAvailable = true
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            var vm = new LibraryDetailsViewModel
            {
                Id = book.Id,
                Name = book.Title,
                Classification = book.Author,
                BuiltDate = new DateTime(book.PublishedYear, 1, 1),
                Crew = book.IsAvailable ? 1 : 0
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            var vm = new LibraryCreateUpdateViewModel
            {
                Id = book.Id,
                Name = book.Title,
                Classification = book.Author,
                BuiltDate = new DateTime(book.PublishedYear, 1, 1),
                Crew = book.IsAvailable ? 1 : 0
            };

            return View("CreateUpdate", vm);
        }

        // Ülesanne 5: Andmete muutmine
        [HttpPost]
        public async Task<IActionResult> Update(Guid id, LibraryCreateUpdateViewModel vm)
        {
            ModelState.Clear();

            // Id võib tulla kas aadressist (/Library/Update/{id}) või vormi peidetud väljalt
            var bookId = vm.Id ?? id;

            if (string.IsNullOrWhiteSpace(vm.Name))
                ModelState.AddModelError(nameof(vm.Name), "Pealkiri on kohustuslik.");
            if (string.IsNullOrWhiteSpace(vm.Classification))
                ModelState.AddModelError(nameof(vm.Classification), "Autor on kohustuslik.");
            if (!vm.BuiltDate.HasValue)
                ModelState.AddModelError(nameof(vm.BuiltDate), "Ilmumiskuupäev on kohustuslik.");

            if (!ModelState.IsValid)
            {
                vm.Id = bookId;
                return View("CreateUpdate", vm);
            }

            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
                return NotFound();

            book.Title = vm.Name.Trim();
            book.Author = vm.Classification.Trim();
            book.PublishedYear = vm.BuiltDate!.Value.Year;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            var vm = new LibraryDeleteViewModel
            {
                Id = book.Id,
                Name = book.Title,
                Classification = book.Author,
                BuiltDate = new DateTime(book.PublishedYear, 1, 1),
                Crew = book.IsAvailable ? 1 : 0
            };

            return View(vm);
        }

        // Ülesanne 5: Kustutamine andmebaasist
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}