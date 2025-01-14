using MovieApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register MovieService for dependency injection
builder.Services.AddHttpClient<MovieService>();

// Add CORS policy to allow requests from the frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin()
                     .AllowAnyHeader()
                     .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

// Map Movies endpoint
app.MapGet("/movies", async (string? title, MovieService movieService) =>
{
    if (string.IsNullOrWhiteSpace(title))
    {
        return Results.BadRequest("Title is required.");
    }

    try
    {
        var movies = await movieService.GetMoviesAsync(title);
        return Results.Ok(movies);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
})
.WithName("GetMovies")
.WithOpenApi();

app.Run();
