using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Lisatud vajalik import ToListAsync jaoks
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.Serviceinterface;
using ShopTARpe25.Data;
using ShopTARpe25.Models.Kindergarden;
using ShopTARpe25.Models.Spaceship;
using System;
using System.Threading.Tasks;

namespace ShopTAR25.Controllers
{
    public class KindergardenController : Controller
    {
        private readonly IKindergardenServices _spaceshipService;
        private readonly ShopTARpe25Context _context;

        public KindergardenController
            (
                IKindergardenServices ispaceshipService,
                ShopTARpe25Context context
            )
        {
            _spaceshipService = ispaceshipService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
          
            var result = await _context.RealEstates
                .Select(x => new KindergardenIndexViewModel 
                {
                    Id = x.Id,
                    GroupName = x.Address, 
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .ToListAsync();

            return View(result);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(KindergardenCreateViewModel vm)
        {
            var dto = new KindergardenDto
            {
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                KindergartenName = vm.KindergartenName,
                TeacherName = vm.TeacherName,
            };

            var result = await _spaceshipService.Create(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            // Päritakse andmed baasist (teenus otsib RealEstates tabelist)
            var spaceship = await _spaceshipService.DetailsAsync(id);

            if (spaceship == null)
            {
                return NotFound();
            }

            // Kuna andmed on tegelikult RealEstates tabelis, siis loeme puuduvad andmed otse sealt,
            // et Details vaade ei jääks tühjaks
            var dbRealEstate = await _context.RealEstates.FirstOrDefaultAsync(x => x.Id == id);

            var vm = new KindergardenDetailsViewModel
            {
                Id = spaceship.Id,
                GroupName = dbRealEstate?.Address ?? "Määramata",
                CreatedAt = spaceship.CreatedAt,
                UpdatedAt = spaceship.UpdatedAt
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var spaceship = await _spaceshipService.DetailsAsync(id);

            if (spaceship == null)
            {
                return NotFound();
            }

            var dbRealEstate = await _context.RealEstates.FirstOrDefaultAsync(x => x.Id == id);

            var vm = new KindergardenUpdateViewModel
            {
                Id = spaceship.Id,
                GroupName = dbRealEstate?.Address ?? "Määramata",
                CreatedAt = spaceship.CreatedAt,
                UpdatedAt = spaceship.UpdatedAt
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(KindergardenUpdateViewModel vm)
        {
            var dto = new KindergardenDto
            {
                Id = vm.Id,
                GroupName = vm.GroupName,
                CreatedAt = vm.CreatedAt ?? DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _spaceshipService.Update(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var spaceship = await _spaceshipService.DetailsAsync(id);

            if (spaceship == null)
            {
                return NotFound();
            }

            var dbRealEstate = await _context.RealEstates.FirstOrDefaultAsync(x => x.Id == id);

            var vm = new KindergardenDeleteViewModel
            {
                Id = spaceship.Id,
                GroupName = dbRealEstate?.Address ?? "Määramata",
                CreatedAt = spaceship.CreatedAt,
                UpdatedAt = spaceship.UpdatedAt
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            await _spaceshipService.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
