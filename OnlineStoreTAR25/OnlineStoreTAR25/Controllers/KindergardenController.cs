using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.Serviceinterface;
using ShopTARpe25.Data;
using ShopTARpe25.Models.Kindergarden;
using ShopTARpe25.Models.Spaceship;
using System;
using System.Threading.Tasks;
using System.Linq; // Lisatud Select ja ToList jaoks mälu-põhiselt

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
            // Loeme andmed otse RealEstates tabelist
            var realEstatesData = await _context.RealEstates.ToListAsync();

            var result = realEstatesData.Select(x => new KindergardenIndexViewModel
            {
                Id = x.Id,
                GroupName = x.Address, // Kasutab Address välja GroupName kuvamiseks
                ChildrenCount = x.ChildrenCount,
                KindergartenName = x.KindergartenName,
                TeacherName = x.TeacherName,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            }).ToList();

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
                GroupName = vm.GroupName, // Liigub teenusesse, kus määratakse Address väljale
                ChildrenCount = vm.ChildrenCount,
                KindergartenName = vm.KindergartenName,
                TeacherName = vm.TeacherName,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _spaceshipService.Create(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var dbRealEstate = await _context.RealEstates.FirstOrDefaultAsync(x => x.Id == id);

            if (dbRealEstate == null)
            {
                return NotFound();
            }

            var vm = new KindergardenDetailsViewModel
            {
                Id = dbRealEstate.Id,
                GroupName = dbRealEstate.Address,
                ChildrenCount = dbRealEstate.ChildrenCount,
                KindergartenName = dbRealEstate.KindergartenName,
                TeacherName = dbRealEstate.TeacherName,
                CreatedAt = dbRealEstate.CreatedAt,
                UpdatedAt = dbRealEstate.UpdatedAt
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var dbRealEstate = await _context.RealEstates.FirstOrDefaultAsync(x => x.Id == id);

            if (dbRealEstate == null)
            {
                return NotFound();
            }

            var vm = new KindergardenUpdateViewModel
            {
                Id = dbRealEstate.Id,
                GroupName = dbRealEstate.Address,
                ChildrenCount = dbRealEstate.ChildrenCount,
                KindergartenName = dbRealEstate.KindergartenName,
                TeacherName = dbRealEstate.TeacherName,
                CreatedAt = dbRealEstate.CreatedAt,
                UpdatedAt = dbRealEstate.UpdatedAt
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
                ChildrenCount = vm.ChildrenCount ?? 0,
                KindergartenName = vm.KindergartenName,
                TeacherName = vm.TeacherName,
                CreatedAt = vm.CreatedAt ?? DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _spaceshipService.Update(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dbRealEstate = await _context.RealEstates.FirstOrDefaultAsync(x => x.Id == id);

            if (dbRealEstate == null)
            {
                return NotFound();
            }

            var vm = new KindergardenDeleteViewModel
            {
                Id = dbRealEstate.Id,
                GroupName = dbRealEstate.Address,
                ChildrenCount = dbRealEstate.ChildrenCount,
                KindergartenName = dbRealEstate.KindergartenName,
                TeacherName = dbRealEstate.TeacherName,
                CreatedAt = dbRealEstate.CreatedAt,
                UpdatedAt = dbRealEstate.UpdatedAt
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
