using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using ShopTARpe25.Models.RealEstate;

namespace ShopTARpe25.Controllers
{
    public class RealEstateController : Controller
    {
        private readonly ShopTARpe25Context _context;
        private readonly IRealEstateServices _realEstateServices;
        private readonly IFileServices _fileService;

        public RealEstateController(
            IRealEstateServices realEstateServices,
            ShopTARpe25Context context,
            IFileServices fileService)
        {
            _context = context;
            _realEstateServices = realEstateServices;
            _fileService = fileService;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var realEstates = await _context.RealEstates.ToListAsync();

            var result = realEstates.Select(x => new RealEstateIndexViewModel
            {
                Id = x.Id,
                Area = x.Area,
                Address = x.Address,
                RoomNumber = x.RoomNumber,
                BuildingType = x.BuildingType,
                CreatedAt = x.CreatedAt,
                ModifiedAt = x.ModifiedAt
            }).ToList();

            return View(result);
        }


        [HttpGet]
        public IActionResult Create()
        {
            RealEstateCreateUpdateViewModel vm = new();

            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RealEstateCreateUpdateViewModel vm)
        {
            var dto = new RealEstateDto
            {
                Area = vm.Area,
                Address = vm.Address,
                RoomNumber = vm.RoomNumber,
                BuildingType = vm.BuildingType,
                Files = vm.Files ?? new List<IFormFile>()
            };

            var result = await _realEstateServices.Create(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var realEstate = await _realEstateServices.DetailAsync(id);

            if (realEstate == null)
            {
                return NotFound();
            }

            var images = await _context.FileToDatabases
                .Where(x => x.RealEstateId == id)
                .Select(y => new RealEstateImageViewModel
                {
                    ImageId = y.Id,
                    RealEstateId = y.RealEstateId,
                    ImageTitle = y.ImageTitle,
                    ImageData = y.ImageData,
                    Image = string.Format("data:image/jpeg;base64,{0}",
                        Convert.ToBase64String(y.ImageData))
                }).ToArrayAsync();

            var vm = new RealEstateDetailsViewModel
            {
                Id = realEstate.Id,
                Area = realEstate.Area,
                Address = realEstate.Address,
                RoomNumber = realEstate.RoomNumber,
                BuildingType = realEstate.BuildingType,
                CreatedAt = realEstate.CreatedAt,
                ModifiedAt = realEstate.ModifiedAt
            };

            vm.Images.AddRange(images);

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var realEstate = await _realEstateServices.DetailAsync(id);

            if (realEstate == null)
            {
                return NotFound();
            }

            var images = await _context.FileToDatabases
                .Where(x => x.RealEstateId == id)
                .Select(y => new RealEstateImageViewModel
                {
                    ImageId = y.Id,
                    RealEstateId = y.RealEstateId,
                    ImageTitle = y.ImageTitle,
                    ImageData = y.ImageData,
                    Image = string.Format("data:image/jpeg;base64,{0}",
                        Convert.ToBase64String(y.ImageData))
                }).ToArrayAsync();

            var vm = new RealEstateCreateUpdateViewModel
            {
                Id = realEstate.Id,
                Area = realEstate.Area,
                Address = realEstate.Address,
                RoomNumber = realEstate.RoomNumber,
                BuildingType = realEstate.BuildingType,
                CreatedAt = realEstate.CreatedAt,
                ModifiedAt = realEstate.ModifiedAt
            };

            vm.Image.AddRange(images);

            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(RealEstateCreateUpdateViewModel vm)
        {
            if (!vm.Id.HasValue)
            {
                return BadRequest();
            }

            var dto = new RealEstateDto
            {
                Id = vm.Id,
                Area = vm.Area,
                Address = vm.Address,
                RoomNumber = vm.RoomNumber,
                BuildingType = vm.BuildingType,
                CreatedAt = vm.CreatedAt,
                Files = vm.Files ?? new List<IFormFile>()
            };

            var result = await _realEstateServices.Update(dto);

            if (result == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Details), new { id = vm.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var realEstate = await _realEstateServices.DetailAsync(id);

            if (realEstate == null)
            {
                return NotFound();
            }

            var images = await _context.FileToDatabases
                .Where(x => x.RealEstateId == id)
                .Select(y => new RealEstateImageViewModel
                {
                    ImageId = y.Id,
                    RealEstateId = y.RealEstateId,
                    ImageTitle = y.ImageTitle,
                    ImageData = y.ImageData,
                    Image = string.Format("data:image/jpeg;base64,{0}",
                        Convert.ToBase64String(y.ImageData))
                }).ToArrayAsync();

            var vm = new RealEstateDeleteViewModel
            {
                Id = realEstate.Id,
                Area = realEstate.Area,
                Address = realEstate.Address,
                RoomNumber = realEstate.RoomNumber,
                BuildingType = realEstate.BuildingType,
                CreatedAt = realEstate.CreatedAt,
                ModifiedAt = realEstate.ModifiedAt
            };

            vm.Images.AddRange(images);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var realEstate = await _realEstateServices.DetailAsync(id);

            if (realEstate == null)
            {
                return NotFound();
            }

            var images = await _context.FileToDatabases
                .Where(x => x.RealEstateId == id)
                .ToListAsync();

            _context.FileToDatabases.RemoveRange(images);

            await _context.SaveChangesAsync();

            await _realEstateServices.Delete(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveImage(RealEstateImageViewModel vm)
        {
            var dto = new FileToDatabaseDto()
            {
                Id = vm.ImageId
            };
            
            var image = await _fileService.RemoveImageFromDatabase(dto);

            if (image == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Update), new { id = vm.RealEstateId });
        }
    }
}