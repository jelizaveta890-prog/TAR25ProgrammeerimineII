
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

        public RealEstateController(
            IRealEstateServices realEstateServices,
            ShopTARpe25Context context)
        {
            _context = context;
            _realEstateServices = realEstateServices;
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


        // CREATE GET
        [HttpGet]
        public IActionResult Create()
        {
            RealEstateCreateUpdateViewModel vm = new();

            return View("CreateUpdate", vm);
        }

        // CREATE POST
        [HttpPost]
        public async Task<IActionResult> Create(
            RealEstateCreateUpdateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateUpdate", vm);
            }

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
                ModelState.AddModelError(
                    "", "Real estate creation failed.");

                return View("CreateUpdate", vm);
            }

            return RedirectToAction(nameof(Index));
        }

        // DETAILS
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var realEstate =
                await _realEstateServices.DetailAsync(id);

            if (realEstate == null)
            {
                return NotFound();
            }

            var images = await FileFromDatabase(id);

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

        // UPDATE GET
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var realEstate =
                await _realEstateServices.DetailAsync(id);

            if (realEstate == null)
            {
                return NotFound();
            }

            var images = await FileFromDatabase(id);

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

        // UPDATE POST
        [HttpPost]
        public async Task<IActionResult> Update(
            RealEstateCreateUpdateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                if (vm.Id.HasValue)
                {
                    vm.Image = (await FileFromDatabase(
                        vm.Id.Value)).ToList();
                }

                return View("CreateUpdate", vm);
            }

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

            return RedirectToAction(nameof(Details),
                new { id = vm.Id });
        }

        // DELETE GET
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var realEstate =
                await _realEstateServices.DetailAsync(id);

            if (realEstate == null)
            {
                return NotFound();
            }

            var images = await FileFromDatabase(id);

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

        // DELETE CONFIRMATION
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var realEstate =
                await _realEstateServices.DetailAsync(id);

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

        // REMOVE ONE IMAGE
        [HttpPost]
        public async Task<IActionResult> RemoveImage(
            Guid imageId, Guid realEstateId)
        {
            var image = await _context.FileToDatabases
                .FirstOrDefaultAsync(x =>
                    x.Id == imageId &&
                    x.RealEstateId == realEstateId);

            if (image == null)
            {
                return NotFound();
            }

            _context.FileToDatabases.Remove(image);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Update),
                new { id = realEstateId });
        }


        private async Task<RealEstateImageViewModel[]> FileFromDatabase(Guid id)
        {
            // Kõigepealt võtame pildid andmebaasist
            var files = await _context.FileToDatabases
                .Where(x => x.RealEstateId == id)
                .ToListAsync();

            // Seejärel teisendame pildid Base64 kujule
            var result = files.Select(y =>
            {
                var extension = Path.GetExtension(y.ImageTitle ?? "")
                    .ToLowerInvariant();

                var contentType = extension switch
                {
                    ".png" => "image/png",
                    ".gif" => "image/gif",
                    ".webp" => "image/webp",
                    ".bmp" => "image/bmp",
                    _ => "image/jpeg"
                };

                return new RealEstateImageViewModel
                {
                    ImageId = y.Id,
                    RealEstateId = y.RealEstateId,
                    ImageData = y.ImageData,
                    ImageTitle = y.ImageTitle,

                    Image = y.ImageData != null
                        ? $"data:{contentType};base64,{Convert.ToBase64String(y.ImageData)}"
                        : null
                };
            }).ToArray();

            return result;
        }

    }
}
