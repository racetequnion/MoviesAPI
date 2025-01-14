using MovieApi.Models;
using System.Text.Json;

namespace MovieApi.Services
{
    public class MovieService
    {
        private readonly HttpClient _httpClient;
        public MovieService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Movie>> GetMoviesAsync(string title)
        {
            var response = await _httpClient.GetAsync($"https://www.omdbapi.com/?s={title}&apikey=9ce69bc2");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var jsonObject = JsonSerializer.Deserialize<JsonElement>(json);

            var movies = jsonObject.GetProperty("Search").EnumerateArray()
                .Select(m => new Movie
                {
                    Title = m.GetProperty("Title").GetString(),
                    Year = m.GetProperty("Year").GetString(),
                    Poster = m.GetProperty("Poster").GetString()
                }).ToList();

            return movies;
        }
    }
}
