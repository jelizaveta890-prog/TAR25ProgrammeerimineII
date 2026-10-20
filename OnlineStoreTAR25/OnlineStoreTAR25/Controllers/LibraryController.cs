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

        // Ülesanne 4: Andmete pärimine LINQ-i abil (filtreerimine ja otsing)
        [HttpGet]
        public IActionResult Index(string searchTitle, int? searchYear, string filterStatus)
        {
            // Alustame kõikide raamatute pärimisega andmebaasist (tabelist dbo.Books)
            var booksQuery = _context.Books.AsQueryable();

            // 1. Leia raamat pealkirja järgi (otsing)
            if (!string.IsNullOrEmpty(searchTitle))
            {
                booksQuery = booksQuery.Where(x => x.Title.Contains(searchTitle));
            }

            // 2. Leia kõik raamatud ilmumisaasta kaudu
            if (searchYear.HasValue)
            {
                booksQuery = booksQuery.Where(x => x.PublishedYear == searchYear.Value);
            }

            // 3. Leia kõik praegu laenutuses olevat raamatud (saadavus on false) või kohal olevad
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

            // Teisendame tulemused LibraryIndexViewModel tüüpi listiks
            var result = booksQuery
                .Select(x => new LibraryIndexViewModel
                {
                    Id = x.Id,
                    Name = x.Title,
                    Classification = x.Author, // x.Author läheb Classification alla
                    BuiltDate = new DateTime(x.PublishedYear, 1, 1),
                    Crew = x.IsAvailable ? 1 : 0, // 1 tähendab saadaval, 0 laenutatud
                    EnginePower = x.PublishedYear
                }).ToList();

            // Säilitame otsinguväärtused input väljade jaoks
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

        // Ülesanne 3: Andmete lisamine (Parandatud: Eemaldatud ModelState.IsValid blokeering!)
        [HttpPost]
        public async Task<IActionResult> Create(LibraryCreateUpdateViewModel vm)
        {
            // Eemaldatud ModelState kontroll, et vanad laeva väljad ei blokeeriks salvestamist
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = vm.Name,
                Author = vm.Classification,
                PublishedYear = vm.BuiltDate.HasValue ? vm.BuiltDate.Value.Year : 2026,
                IsAvailable = true // Raamatu saadavus on alguses alati true
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

        // Ülesanne 5: Andmete muutmine (raamatu pealkirja jm muutmine)
        [HttpPost]
        public async Task<IActionResult> Update(LibraryCreateUpdateViewModel vm)
        {
            var book = await _context.Books.FindAsync(vm.Id);

            if (book != null)
            {
                book.Title = vm.Name;
                book.Author = vm.Classification;
                book.PublishedYear = vm.BuiltDate.HasValue ? vm.BuiltDate.Value.Year : 2026;

                _context.Books.Update(book);
                await _context.SaveChangesAsync();
            }

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
