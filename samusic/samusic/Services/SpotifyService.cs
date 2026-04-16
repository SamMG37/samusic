using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

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

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://accounts.spotify.com/api/token"
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Basic", auth);

            request.Content = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    { "grant_type", "client_credentials" }
                }
            );

            var response = await client.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Spotify token request failed: " + json);
            }

            var tokenObject = JObject.Parse(json);
            var accessToken = tokenObject["access_token"]?.ToString();

            if (string.IsNullOrEmpty(accessToken))
            {
                throw new Exception("Spotify token missing. Response was: " + json);
            }

            return accessToken;
        }

        public async Task<string> Search(string query)
        {
            var token = await GetToken();

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"https://api.spotify.com/v1/search?q={Uri.EscapeDataString(query)}&type=track"
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Spotify search failed: " + json);
            }

            return json;
        }

        public async Task<string> GetTracksByGenre(string genre)
        {
            var token = await GetToken();

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"https://api.spotify.com/v1/search?q=genre:\"{Uri.EscapeDataString(genre)}\"&type=track&limit=10"
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Spotify genre search failed: " + json);
            }

            return json;
        }

        public async Task<string> GetTrackById(string trackId)
        {
            var token = await GetToken();

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"https://api.spotify.com/v1/tracks/{trackId}"
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Spotify track lookup failed: " + json);
            }

            return json;
        }
    }
}

