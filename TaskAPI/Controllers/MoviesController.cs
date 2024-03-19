using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskAPI.Context;
using TaskAPI.Entities;
using TaskAPI.Models;

namespace TaskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly AppDbContext _appDbContext; // field
        public MoviesController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext; // dependency injection object
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_appDbContext.Movies);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var movie = _appDbContext.Movies.Find(id);
            if (movie == null)
            {
                return NotFound();
            }
            movie.Views++;
            _appDbContext.SaveChanges();
            return Ok(movie);
        }

        [HttpGet("getMoviesBySearch")]
        public IActionResult GetMoviesBySearch(string text)
        {
            return Ok(_appDbContext.Movies.Where(movie => movie.Name.Contains(text) == true));
        }

        [HttpGet("getMoviesByYear")]
        public IActionResult GetMoviesByYear(ushort year)
        {
            return Ok(_appDbContext.Movies.Where(movie => movie.Year == year));
        }

        [HttpGet("getMoviesByFilter")]
        public IActionResult GetMoviesByFilter(int categoryId, ushort year, int imdbPoint)
        {
            var movies = _appDbContext.Movies.Where(movie => movie.Categories.FirstOrDefault(c => c.Id == categoryId) != null && movie.Year == year && movie.ImdbPoint >= imdbPoint);
            return Ok(movies);
        }

        [HttpPost]
        public void Post(MovieAddDTO newMovie)
        {
            Movie movie = new()
            {
                Name = newMovie.Name,
                Description = newMovie.Description,
                Year = newMovie.Year,
                Language = newMovie.Language,
                ImdbPoint = newMovie.ImdbPoint,
                Categories = newMovie.Categories,
                Directors = newMovie.Directors,
                Actors = newMovie.Actors
            };
            _appDbContext.Movies.Add(movie);
            _appDbContext.SaveChanges();
        }

        [HttpPatch("{id}")]
        public void Put(int id, MovieUpdateDTO movie) // DTO - Data  Transfer Object
        {
            Movie updateaMovie = _appDbContext.Movies.Find(id);
            updateaMovie.Name = movie.Name;
            updateaMovie.Description = movie.Description;
            updateaMovie.Language = movie.Language;
            updateaMovie.Year = movie.Year;
            updateaMovie.ImdbPoint = movie.ImdbPoint;
            _appDbContext.SaveChanges();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Movie deleteMovie = _appDbContext.Movies.Find(id);
            _appDbContext.Movies.Remove(deleteMovie);
            _appDbContext.SaveChanges();
        }
    }
}