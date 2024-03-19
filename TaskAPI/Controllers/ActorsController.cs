using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskAPI.Context;
using TaskAPI.Entities;
using TaskAPI.Models;

namespace TaskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly AppDbContext _appDbContext; // field
        public ActorsController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext; // dependency injection object
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_appDbContext.Actors.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var actor = _appDbContext.Actors.Find(id);
            if (actor == null)
            {
                return NotFound();
            }
            return Ok(actor);
        }

        [HttpGet("getMoviesByActorId")]
        public IActionResult GetMoviesByActorId(int id)
        {
            var movies = _appDbContext.Movies.Where(movie => movie.Actors.FirstOrDefault(c => c.Id == id) != null);
            if (movies == null)
            {
                return NotFound();
            }
            return Ok(movies);
        }

        [HttpPost]
        public void Post(string firstName, string lastName)
        {
            Actor actor = new() { FirstName = firstName, LastName = lastName };
            _appDbContext.Actors.Add(actor);
            _appDbContext.SaveChanges();
        }

        [HttpPatch("{id}")]
        public void Put(int id, ActorUpdateDTO actor) // DTO - Data  Transfer Object
        {
            Actor updateaActor = _appDbContext.Actors.Find(id);
            updateaActor.FirstName = actor.FirstName;
            updateaActor.LastName = actor.LastName;
            _appDbContext.SaveChanges();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Actor deleteActor = _appDbContext.Actors.Find(id);
            _appDbContext.Actors.Remove(deleteActor);
            _appDbContext.SaveChanges();
        }
    }
}
