using Microsoft.AspNetCore.Mvc;
using ScrapperCron.Services;

namespace ScraperAdmin.Controllers
{
    public class ScraperController : Controller
    {
        ICronService _cronService;
        public ScraperController(ICronService _cronService) {

            ICronService cronService = _cronService;
        }

        public IActionResult Index()
        {
            return View("Scraper");
        }

        public async Task<IActionResult> ExecuteOnce() {
            try
            {
                await _cronService.ExecuteOnce();
            }
            catch (Exception)
            {

                throw;
            }         

            return Ok();
        }
    }
}
