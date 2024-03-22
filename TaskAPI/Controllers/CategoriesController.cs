using Microsoft.AspNetCore.Mvc;
using TaskAPI.Context;
using TaskAPI.Entities;
using TaskAPI.Models;

// For mre information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase // test
    {
        private readonly AppDbContext _appDbContext; // field
        public CategoriesController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext; // dependency injection object
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_appDbContext.Categories.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var category = _appDbContext.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpGet("getMoviesByCategoryId")]
        public IActionResult GetMoviesByCategoryId(int id)
        {
            var movies = _appDbContext.Movies.Where(movie => movie.Categories.FirstOrDefault(c => c.Id == id) != null).OrderByDescending(movie => movie.Year);
            if (movies == null)
            {
                return NotFound();
            }
            return Ok(movies);
        }

        [HttpPost]
        public void Post(string name)
        {
            Category category = new() { Name = name };
            _appDbContext.Categories.Add(category);
            _appDbContext.SaveChanges();
        }
        [HttpPatch("{id}")]
        public void Put(int id, CategoryUpdateDTO category) // DTO - Data  Transfer Object
        {
            Category updateCategory = _appDbContext.Categories.FirstOrDefault(category => category.Id == id);
            updateCategory.Name = category.Name;
            _appDbContext.SaveChanges();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Category deleteCategory = _appDbContext.Categories.FirstOrDefault(category => category.Id == id);
            _appDbContext.Categories.Remove(deleteCategory);
            _appDbContext.SaveChanges();
        }
    }
}
