using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using ShopTARpe25.Models.Library;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShopTARpe25.Controllers
{
    public class LibraryController : Controller
    {
        private readonly ILibraryServices _libraryService;
        private readonly ShopTARpe25Context _context;

        public LibraryController
            (
                ILibraryServices libraryService,
                ShopTARpe25Context context
            )
        {
            _libraryService = libraryService;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var result = _context.Books
                .Select(x => new LibraryIndexViewModel
                {
                    Id = x.Id,
                    Name = x.Title,
                    Classification = x.Author,
                    BuiltDate = new DateTime(x.PublishedYear, 1, 1),
                    Crew = x.IsAvailable ? 1 : 0,
                    EnginePower = 0
                }).ToList();

            return View(result);
        }


        [HttpGet]
        public IActionResult Create()
        {
            LibraryCreateUpdateViewModel vm = new();
            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(LibraryCreateUpdateViewModel vm)
        {
            var dto = new LibraryDto
            {
                Id = vm.Id,
                Name = vm.Name,
                Classification = vm.Classification,
                BuildDate = vm.BuiltDate,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower
            };

            var result = await _libraryService.Create(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var libraryItem = await _libraryService.DetailsAsync(id);

            if (libraryItem == null)
            {
                return NotFound();
            }

            var vm = new LibraryDetailsViewModel
            {
                Id = libraryItem.Id,
                Name = libraryItem.Name,
                Classification = libraryItem.Classification,
                BuiltDate = libraryItem.BuiltDate,
                Crew = libraryItem.Crew,
                EnginePower = libraryItem.EnginePower,
                CreatedAt = libraryItem.CreatedAt,
                ModifiedAt = libraryItem.ModifiedAt
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var libraryItem = await _libraryService.DetailsAsync(id);

            if (libraryItem == null)
            {
                return NotFound();
            }

            var vm = new LibraryCreateUpdateViewModel
            {
                Id = libraryItem.Id,
                Name = libraryItem.Name,
                Classification = libraryItem.Classification,
                BuiltDate = libraryItem.BuiltDate,
                Crew = libraryItem.Crew,
                EnginePower = libraryItem.EnginePower,
                CreatedAt = libraryItem.CreatedAt,
                ModifiedAt = libraryItem.ModifiedAt
            };

            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(LibraryCreateUpdateViewModel vm)
        {
            var dto = new LibraryDto
            {
                Id = vm.Id,
                Name = vm.Name,
                Classification = vm.Classification,
                BuildDate = vm.BuiltDate,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower
            };

            var result = await _libraryService.Update(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var libraryItem = await _libraryService.DetailsAsync(id);

            if (libraryItem == null)
            {
                return NotFound();
            }

            var vm = new LibraryDeleteViewModel
            {
                Id = libraryItem.Id,
                Name = libraryItem.Name,
                Classification = libraryItem.Classification,
                BuiltDate = libraryItem.BuiltDate,
                Crew = libraryItem.Crew,
                EnginePower = libraryItem.EnginePower,
                CreatedAt = libraryItem.CreatedAt,
                ModifiedAt = libraryItem.ModifiedAt
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var result = await _libraryService.Delete(id);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
