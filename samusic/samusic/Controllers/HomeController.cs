using Microsoft.AspNetCore.Mvc;
using samusic.Models;
using samusic.Services;
using System.Diagnostics;

namespace samusic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SpotifyService spotify;

        public HomeController(ILogger<HomeController> logger, SpotifyService spotifyService)
        {
            _logger = logger;
            spotify = spotifyService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> NewReleases()
        {
            var results = await spotify.GetNewReleases();
            return Content(results, "application/json");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}