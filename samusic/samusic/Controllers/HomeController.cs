using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using samusic.Data;
using samusic.Models;
using samusic.Services;
using System.Diagnostics;

namespace samusic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SpotifyService spotify;
        private readonly ApplicationDbContext context;

        public HomeController(ILogger<HomeController> logger, SpotifyService spotifyService, ApplicationDbContext dbContext)
        {
            _logger = logger;
            spotify = spotifyService;
            context = dbContext;
        }

        public IActionResult Index()
        {
            var randomReviews = context.TrackReviews
                .ToList()
                .OrderBy(r => Guid.NewGuid())
                .Take(6)
                .ToList();

            ViewBag.RandomReviews = randomReviews;

            return View();
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