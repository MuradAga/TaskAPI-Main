using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using System.IO;
using System.Linq;
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
            var movies = _appDbContext.Movies.ToList();

            foreach (var movie in movies)
            {
                movie.Directors = _appDbContext.Directors.Where(d => d.Movies.FirstOrDefault(m => m.Id == movie.Id) != null).ToList();
                movie.Actors = _appDbContext.Actors.Where(a => a.Movies.FirstOrDefault(m => m.Id == movie.Id) != null).ToList();
                movie.Categories = _appDbContext.Categories.Where(c => c.Movies.FirstOrDefault(m => m.Id == movie.Id) != null).ToList();
            }

            return Ok(movies);
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

        [HttpGet("getMostViewedFilms")]
        public IActionResult GetMostViewedFilms()
        {
            var movies = _appDbContext.Movies.OrderByDescending(movie => movie.Views);
            return Ok(movies);
        }

        [HttpPost]
        public void Post(MovieAddDTO newMovie)
        {
            List<Category> categories = _appDbContext.Categories.Where(c => newMovie.CategoryIds.Contains(c.Id)).ToList();
            List<Director> directors = _appDbContext.Directors.Where(d => newMovie.DirectorIds.Contains(d.Id)).ToList(); ;
            List<Actor> actors = _appDbContext.Actors.Where(a => newMovie.ActorIds.Contains(a.Id)).ToList(); ;

            Movie movie = new()
            {
                Name = newMovie.Name,
                Description = newMovie.Description,
                CoverUrl = newMovie.CoverUrl,
                TrailerUrl = newMovie.TrailerUrl,
                Year = newMovie.Year,
                Language = newMovie.Language,
                ImdbPoint = newMovie.ImdbPoint,
                Categories = categories,
                Directors = directors,
                Actors = actors
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
            updateaMovie.CoverUrl = movie.CoverUrl;
            updateaMovie.TrailerUrl = movie.TrailerUrl;
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