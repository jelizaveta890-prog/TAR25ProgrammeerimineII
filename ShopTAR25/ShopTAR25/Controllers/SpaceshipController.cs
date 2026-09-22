using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using ShopTAR25.Models.Spaceship;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.Servicesinterface;
using ShopTARpe25.Data;

namespace ShopTAR25.Controllers
{
    public class SpaceshipController : Controller
    {
        private readonly ISpaceshipServices _spaceshipServices;

        private readonly ShopTARpe25Context _context;
        //teha constructor et saaks kasutada teenust, mis on 
        //defineeritud ISpaceshipServices liideses
        //lisage Context
        public SpaceshipController
            (
                ISpaceshipServices spaceshipServices,
                ShopTARpe25Context context
            )
        {
            _spaceshipServices = spaceshipServices;
            _context = context;
        }


        public IActionResult Index()
        {

            var result = _context.Spaceships
              .Select(x => new SpaceshipIndexViewModel
              {
                  Id = x.Id,
                  Name = x.Name,
                  Classification = x.Classification,
                  BuiltDate = x.BuiltDate,
                  Crew = x.Crew,
                  Enginepower = x.EnginePower
              }).ToList();

            return View(result);
        }

        //kui kasutja klikib "Create" nuppu, siis see meetod käivitatakse 
        //tagastab kasutaja vormi, kuhu saab sisestada andmed
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //kui oled teinud vormi, soiis see meetod käivitakse
        //saab andmed serverisse, kus need salvestatakse andmebaasi
        [HttpPost]
        public async Task<IActionResult> Create(SpaceshipCreateViewModel vm)
        {

            //luua vaheinstains, mis sisaldab andmeid, mis on saadud vormis
            //need andmed tuleb edasi saata dto-sse, mis on mõeldud andmebaasi salvestamiseks


            var dto = new SpaceshipDto
            {
                Name = vm.Name,
                Classification = vm.Classification,
                BuiltDate = vm.BuiltDate,
                Crew = vm.Crew,
                EnginePower = vm.Enginepower
            };

            var result = await _spaceshipServices.Create(dto);


            return RedirectToAction(nameof(Index));
        }

        //tuleb teha Details meetod 
        //see kutsub välja interfacest service meetodi

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {

            var spaceship = await _spaceshipServices.DetailsAsync(id);

            //veakäsitlus
            //suunab vatele NotFound, kui andmed ei ole
            if (spaceship == null)
            {
                return NotFound();
            }

            //tuleb teha viewModel ja see siin välja kutsuda 
            //ära map-ida vm ja domain
            var vm = new SpaceshipDetailsViewModel();

            vm.Id = spaceship.Id;
            vm.Name = spaceship.Name;
            vm.Classification = spaceship.Classification;
            vm.BuiltDate = spaceship.BuiltDate;
            vm.Crew = spaceship.Crew;
            vm.EnginePower = spaceship.EnginePower;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.ModifiedAt = spaceship.ModifiedAt;

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {

            var spaceship = await _spaceshipServices.DetailsAsync(id);


            if (spaceship == null)
            {
                return NotFound();
            }


            var vm = new SpaceshipUpdateViewModel();

            vm.Id = spaceship.Id;
            vm.Name = spaceship.Name;
            vm.Classification = spaceship.Classification;
            vm.BuiltDate = spaceship.BuiltDate;
            vm.Crew = spaceship.Crew;
            vm.EnginePower = spaceship.EnginePower;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.ModifiedAt = spaceship.ModifiedAt;

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SpaceshipUpdateViewModel vm)
        {

            var dto = new SpaceshipDto()
            {
                Id = vm.Id,
                Name = vm.Name,
                Classification = vm.Classification,
                BuiltDate = vm.BuiltDate,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower,
                CreatedAt = vm.CreatedAt,
                ModifiedAt = vm.ModifiedAt
            };

            var result = await _spaceshipServices.Update(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }


            return RedirectToAction(nameof(Index));
        }
        //-------------------------------------DELETE

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {

            var spaceship = await _spaceshipServices.DetailsAsync(id);


            if (spaceship == null)
            {
                return NotFound();
            }


            var vm = new SpaceshipDeleteViewModel();

            vm.Id = spaceship.Id;
            vm.Name = spaceship.Name;
            vm.Classification = spaceship.Classification;
            vm.BuiltDate = spaceship.BuiltDate;
            vm.Crew = spaceship.Crew;
            vm.Enginepower = spaceship.EnginePower;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.ModifiedAt = spaceship.ModifiedAt;

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            
            var result = await _spaceshipServices.Delete(id);


            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }


            return RedirectToAction(nameof(Index));
        }

    }
}
