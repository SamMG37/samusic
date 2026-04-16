using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using samusic.Data;
using samusic.Models;
using samusic.Services;
using System.Diagnostics;

namespace samusic.Controllers
{
    // Defines the HomeController, handles requests
    public class HomeController : Controller
    {
        // Logger records application events, debuggs information
        private readonly ILogger<HomeController> _logger;

        // Service used for Spotify-related operations
        private readonly SpotifyService spotify;

        // Database context used to access application data stored in SQL Server
        private readonly ApplicationDbContext context;

        // Constructor for dependency injection.
        public HomeController(ILogger<HomeController> logger, SpotifyService spotifyService, ApplicationDbContext dbContext)
        {
            _logger = logger;
            spotify = spotifyService;
            context = dbContext;
        }

        // Handles requests to the home page
        public IActionResult Index()
        {
            // Retrieves all track reviews from the database and randomises
            var randomReviews = context.TrackReviews
                .ToList()
                .OrderBy(r => Guid.NewGuid())
                .Take(6)
                .ToList();

            ViewBag.RandomReviews = randomReviews;

            // Returns Home/Index view
            return View();
        }

        // Handles requests to the privacy page
        public IActionResult Privacy()
        {
            return View();
        }

        // Handles errors and prevents the error response from being cached
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}