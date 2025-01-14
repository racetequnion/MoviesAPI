using Microsoft.AspNetCore.Mvc;
using MovieApi.Models;
using MovieApi.Services;

[ApiController]
[Route("[controller]")]
public class MoviesController : ControllerBase
{
    private readonly MovieService _movieService;

    public MoviesController(MovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return BadRequest("Title is required.");
        var movies = await _movieService.GetMoviesAsync(title);
        return Ok(movies);
    }
}
