using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskAPI.Context;
using TaskAPI.Entities;
using TaskAPI.Models;

namespace TaskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListsController : ControllerBase
    {
        private readonly AppDbContext _appDbContext; // field
        public ListsController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext; // dependency injection object
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_appDbContext.Lists.ToList());
        }

        [HttpGet]
        public IActionResult GetOrderByLists()
        {
            var lists = _appDbContext.Lists.OrderByDescending(list => list.CreatedAt);
            return Ok(lists);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var list = _appDbContext.Lists.Find(id);
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }

        [HttpGet("getMoviesByListId")]
        public IActionResult GetMoviesByListId(int id)
        {
            var movies = _appDbContext.Movies.Where(movie => movie.Lists.FirstOrDefault(c => c.Id == id) != null);
            if (movies == null)
            {
                return NotFound();
            }
            return Ok(movies);
        }

        [HttpPost]
        public void Post(ListAddDTO newList)
        {
            List<Movie> movies = _appDbContext.Movies.Where(c => newList.MovieIds.Contains(c.Id)).ToList();

            List list = new()
            {
                Name = newList.Name,
                ShortDescription = newList.ShortDescription,
                LongDescription = newList.LongDescription,
                Movies = movies
            };
            _appDbContext.Lists.Add(list);
            _appDbContext.SaveChanges();
        }

        [HttpPatch("{id}")]
        public void Put(int id, ListUpdateDTO list) // DTO - Data  Transfer Object
        {
            List<Movie> movies = _appDbContext.Movies.Where(c => list.MovieIds.Contains(c.Id)).ToList();
            List updateaList = _appDbContext.Lists.Find(id);
            updateaList.Name = list.Name;
            updateaList.ShortDescription = list.ShortDescription;
            updateaList.LongDescription = list.LongDescription;
            updateaList.Movies = movies;
            _appDbContext.SaveChanges();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            List deleteList = _appDbContext.Lists.Find(id);
            _appDbContext.Lists.Remove(deleteList);
            _appDbContext.SaveChanges();
        }
    }
}
