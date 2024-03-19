using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using TaskAPI.Context;
using TaskAPI.Entities;
using TaskAPI.Models;

namespace TaskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorsController : ControllerBase
    {
        private readonly AppDbContext _appDbContext; // field
        public DirectorsController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext; // dependency injection object
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_appDbContext.Directors.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var director = _appDbContext.Directors.Find(id);
            if (director == null)
            {
                return NotFound();
            }
            return Ok(director);
        }

        [HttpGet("getMoviesByDirectorId")]
        public IActionResult GetMoviesByDirectorId(int id)
        {
            var movies = _appDbContext.Movies.Where(movie => movie.Directors.FirstOrDefault(d => d.Id == id) != null);
            if (movies == null)
            {
                return NotFound();
            }
            return Ok(movies);
        }

        [HttpPost]
        public void Post(string firstName, string lastName)
        {
            Director director = new() { FirstName = firstName, LastName = lastName };
            _appDbContext.Directors.Add(director);
            _appDbContext.SaveChanges();
        }

        [HttpPatch("{id}")]
        public void Put(int id, DirectorUpdateDTO director) // DTO - Data  Transfer Object
        {
            Director updateaActor = _appDbContext.Directors.Find(id);
            updateaActor.FirstName = director.FirstName;
            updateaActor.LastName = director.LastName;
            _appDbContext.SaveChanges();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Director deleteDirector = _appDbContext.Directors.Find(id);
            _appDbContext.Directors.Remove(deleteDirector);
            _appDbContext.SaveChanges();
        }
    }
}
