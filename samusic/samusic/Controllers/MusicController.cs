using Microsoft.AspNetCore.Mvc;
using samusic.Services;

namespace samusic.Controllers
{

    public class MusicController : Controller
    {

        SpotifyService spotify = new SpotifyService();

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Search(string query)
        {
            var results = await spotify.Search(query);

            return Content(results, "application/json");
        }

        public async Task<IActionResult> Trending()
        {
            var results = await spotify.GetTrending();

            return Content(results, "application/json");
        }
    }
}