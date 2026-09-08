using Microsoft.AspNetCore.Mvc;
using ShopTAR25.Models.Spaceship;

namespace ShopTAR25.Controllers
{
    public class SpaceshipController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //kui kasutja klikib "Create" nuppu, siis see meetod käivitatakse 
        //tagastab kasutaja vormi, kuhu saab sisestada andmed
        [HttpPost]
        public IActionResult Create(SpaceshipCreateViewModel vm)
        {
            return View();
        }

        //kui oled teinud vormi, soiis see meetod käivitakse
        //saab andmed serverisse, kus need salvestatakse andmebaasi
        [HttpPost]
        public async Task<IActionResult> Create()
        {
            return RedirectToAction(nameof(Index));
        }

    }
}
