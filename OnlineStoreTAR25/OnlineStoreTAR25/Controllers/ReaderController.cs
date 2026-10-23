using Microsoft.AspNetCore.Mvc;
using ShopTARpe25.Core.ServiceInterface;

namespace ShopTARpe25.Controllers
{
    public class ReaderController : Controller
    {
        private readonly IBookServices _bookServices;

        public ReaderController(IBookServices bookServices)
        {
            _bookServices = bookServices;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _bookServices.GetReadersAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(string fullName, string email)
        {
            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email))
            {
                TempData["Error"] = "Nimi ja e-post on kohustuslikud.";
                return RedirectToAction(nameof(Index));
            }

            await _bookServices.AddReaderAsync(fullName, email);
            TempData["Success"] = "Lugeja lisatud.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Loans(Guid id)
        {
            var readers = await _bookServices.GetReadersAsync();
            var reader = readers.FirstOrDefault(r => r.Id == id);
            if (reader == null) return NotFound();

            ViewBag.ReaderName = reader.FullName;
            return View(await _bookServices.GetLoansByReaderAsync(id));
        }
    }
}