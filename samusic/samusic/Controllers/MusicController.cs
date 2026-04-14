using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using samusic.Data;
using samusic.Models;
using samusic.Services;
using System.Security.Claims;

namespace samusic.Controllers
{
    public class MusicController : Controller
    {
        private readonly SpotifyService spotify;
        private readonly ApplicationDbContext context;

        public MusicController(SpotifyService spotifyService, ApplicationDbContext dbContext)
        {
            spotify = spotifyService;
            context = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Search(string query)
        {
            var results = await spotify.Search(query);
            return Content(results, "application/json");
        }

        public async Task<IActionResult> Genre(string genre)
        {
            var results = await spotify.GetTracksByGenre(genre);
            return Content(results, "application/json");
        }

        [Authorize]
        [HttpPost]

        public IActionResult SaveFavourite([FromBody] FavouriteSong favourite)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            bool alreadySaved = context.FavouriteSongs.Any(f =>
                f.UserId == userId &&
                f.SpotifyTrackId == favourite.SpotifyTrackId);

            if (alreadySaved)
            {
                return Json(new { success = false, message = "Song already saved." });
            }

            favourite.UserId = userId;
            favourite.SavedAt = DateTime.UtcNow;

            context.FavouriteSongs.Add(favourite);
            context.SaveChanges();

            return Json(new { success = true, message = "Song saved to favourites." });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveFavourite(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var favourite = context.FavouriteSongs
                .FirstOrDefault(f => f.Id == id && f.UserId == userId);

            if (favourite == null)
            {
                return NotFound();
            }

            context.FavouriteSongs.Remove(favourite);
            context.SaveChanges();

            TempData["SuccessMessage"] = "Song removed from favourites.";

            return RedirectToAction("Favourites");
        }

        [Authorize]
        public IActionResult Favourites()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var favourites = context.FavouriteSongs
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.SavedAt)
                .ToList();

            return View(favourites);
        }
    }
}