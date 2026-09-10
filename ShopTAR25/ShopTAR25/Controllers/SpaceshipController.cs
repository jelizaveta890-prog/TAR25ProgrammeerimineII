using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using ShopTAR25.Models.Spaceship;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.Servicesinterface;

namespace ShopTAR25.Controllers
{
    public class SpaceshipController : Controller
    {
        private readonly ISpaceshipServices _spaceshipServices;


        public SpaceshipController
            (
                ISpaceshipServices spaceshipServices
            )
        {
            _spaceshipServices = spaceshipServices;
        }

        //teha constructor et saaks kasutada teenust, mis on 
        //defineeritud ISpaceshipServices liideses
        public IActionResult Index()
        {
            return View();
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

            
            var dto = new  SpaceshipDto
            {
                Name = vm.Name,
                Classification = vm.Classification,
                BuiltDate = vm.BuildDate,
                Crew = vm.Crew,
                EnginePower = vm.Egienepower
            };

            var result = await _spaceshipServices.Create(dto);
            

            return RedirectToAction(nameof(Index));
        }
    }
}
