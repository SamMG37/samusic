using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace samusic.Services
{

    public class SpotifyService
    {

        private readonly HttpClient client = new HttpClient();

        private string clientId = "213f0d754f974770833165fa60f6843b";

        private string clientSecret = "a5710ad8c5314647a133cb426c418800";

        public async Task<string> GetToken()
        {

            var auth = Convert.ToBase64String(

                Encoding.UTF8.GetBytes(clientId + ":" + clientSecret)
            
            );

            var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");

            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", auth);

            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"grant_type","client_credentials"}
            }
            
            );

            var response = await client.SendAsync(request);

            var json = await response.Content.ReadAsStringAsync();

            dynamic token = JsonConvert.DeserializeObject(json);

            return token.access_token;

        }

        public async Task<string> Search(string query)
        {

            var token = await GetToken();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"https://api.spotify.com/v1/search?q={query}&type=track&limit=25");

            var result = await response.Content.ReadAsStringAsync();

            Console.WriteLine(result);

            return result;

        }

        public async Task<string> GetTrending()
        {

            var token = await GetToken();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("https://api.spotify.com/v1/browse/new-releases?limit=25");

            var result = await response.Content.ReadAsStringAsync();

            Console.WriteLine(result);

            return result;

        }

    }
}