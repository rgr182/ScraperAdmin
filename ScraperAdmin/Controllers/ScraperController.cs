using Microsoft.AspNetCore.Mvc;

namespace ScraperAdmin.Controllers
{
    public class ScraperController : Controller
    {
        public IActionResult Index()
        {
            return View("Scraper");
        }
    }
}
