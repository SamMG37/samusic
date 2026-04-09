using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace samusic.Services
{

    public class SpotifyService
    {

        private HttpClient client = new HttpClient();

        private string clientId = "PUT CLIENT ID";

        private string clientSecret = "PUT SECRET";

        public async Task<string> GetToken()
        {

            var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes(clientId + ":" + clientSecret));

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);

            var request = new FormUrlEncodedContent(

                new Dictionary<string, string>
                {
                    {"grant_type","client_credentials"}
                }
            
            );

            var response = await client.PostAsync("https://accounts.spotify.com/api/token", request);

            var json = await response.Content.ReadAsStringAsync();

            dynamic token = JsonConvert.DeserializeObject(json);

            return token.access_token;

        }

        public async Task<string> Search(string query)
        {

            var token = await GetToken();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"https://api.spotify.com/v1/search?q={query}&type=track&limit=25"

            );

            return await response.Content.ReadAsStringAsync();

        }

        public async Task<string> GetTrending()
        {

            var token = await GetToken();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("https://api.spotify.com/v1/browse/new-releases?limit=25");

            return await response.Content.ReadAsStringAsync();

        }

    }

}
