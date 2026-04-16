using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using samusic.Data;
using samusic.Models;
using samusic.Services;
using System.Security.Claims;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace samusic.Controllers
{
    // Handles requests: searching, browsing genres, saving favourites, managing reviews
    public class MusicController : Controller
    {
        // Service to interact with Spotify data
        private readonly SpotifyService spotify;

        // Database context to access favourites and reviews
        private readonly ApplicationDbContext context;

        // Constructor for dependency injection
        public MusicController(SpotifyService spotifyService, ApplicationDbContext dbContext)
        {
            spotify = spotifyService;
            context = dbContext;
        }

        // Loads main Music page
        public IActionResult Index()
        {
            return View();
        }

        // Searches Spotify for tracks based on the user's query
        public async Task<IActionResult> Search(string query)
        {
            var results = await spotify.Search(query);
            return Content(results, "application/json");
        }

        // Retrieves tracks from Spotify based on selected genre
        public async Task<IActionResult> Genre(string genre)
        {
            var results = await spotify.GetTracksByGenre(genre);
            return Content(results, "application/json");
        }

        [Authorize]
        [HttpPost]

        // Saves selected song to logged-in user's favourites
        public IActionResult SaveFavourite([FromBody] FavouriteSong favourite)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            // Checks whether song has already been saved by user
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

        // Removes selected favourite from logged-in user's saved list
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

        // Loads logged-in user's favourites page
        public IActionResult Favourites()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            // Retrieves user's favourites in most-recent-first order
            var favourites = context.FavouriteSongs
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.SavedAt)
                .ToList();

            return View(favourites);
        }

        // Loads reviews page for selected track
        public async Task<IActionResult> Reviews(string trackId)
        {
            if (string.IsNullOrWhiteSpace(trackId))
            {
                return RedirectToAction("Index");
            }

            // Retrieves detailed track information from Spotify
            var trackJson = await spotify.GetTrackById(trackId);
            var trackObject = JObject.Parse(trackJson);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieves all reviews for the selected track
            var reviews = context.TrackReviews
                .Where(r => r.SpotifyTrackId == trackId)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();

            // Combines Spotify track details and database reviews into one view model
            var model = new TrackReviewsView
            {
                SpotifyTrackId = trackId,
                TrackName = trackObject["name"]?.ToString() ?? "",
                ArtistNames = string.Join(", ",
                    trackObject["artists"]?.Select(a => a?["name"]?.ToString()).Where(n => !string.IsNullOrWhiteSpace(n))!
                    ?? Enumerable.Empty<string>()),
                AlbumName = trackObject["album"]?["name"]?.ToString() ?? "",
                AlbumImageUrl = trackObject["album"]?["images"]?.First?["url"]?.ToString() ?? "",
                SpotifyUrl = trackObject["external_urls"]?["spotify"]?.ToString() ?? "",
                AverageRating = reviews.Any() ? Math.Round(reviews.Average(r => r.Rating), 1) : 0,
                ReviewCount = reviews.Count,
                HasReviewed = userId != null && reviews.Any(r => r.UserId == userId),
                Reviews = reviews
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        // Adds new review for selected track
        public IActionResult AddReview(
            string spotifyTrackId,
            string trackName,
            string artistNames,
            string albumName,
            string albumImageUrl,
            string spotifyUrl,
            int rating,
            string comment)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            // Prevents same user from reviewing same track more than once
            bool alreadyReviewed = context.TrackReviews.Any(r =>
                r.SpotifyTrackId == spotifyTrackId &&
                r.UserId == userId);

            if (alreadyReviewed)
            {
                TempData["ReviewMessage"] = "You have already reviewed this track.";
                return RedirectToAction("Reviews", new { trackId = spotifyTrackId });
            }

            var review = new TrackReview
            {
                SpotifyTrackId = spotifyTrackId,
                TrackName = trackName,
                ArtistNames = artistNames,
                AlbumName = albumName,
                AlbumImageUrl = albumImageUrl,
                SpotifyUrl = spotifyUrl,
                Rating = rating,
                Comment = comment ?? "",
                UserId = userId,
                UserEmail = User.Identity?.Name ?? "Unknown user",
                CreatedAt = DateTime.UtcNow
            };

            context.TrackReviews.Add(review);
            context.SaveChanges();

            TempData["ReviewMessage"] = "Review added successfully.";

            return RedirectToAction("Reviews", new { trackId = spotifyTrackId });
        }

        // Loads logged-in user's own reviews
        public IActionResult MyReviews()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            // Retrieves all reviews made by current user
            var reviews = context.TrackReviews
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();

            return View(reviews);
        }
    }
}