using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskAPI.Context;
using TaskAPI.Entities;
using TaskAPI.Models;

namespace TaskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly AppDbContext _appDbContext; // field
        public MessagesController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext; // dependency injection object
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_appDbContext.Lists.ToList());
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

        [HttpPost]
        public void Post(string name, string email, string topic, string messageText)
        {
            Message message = new()
            {
                Name = name,
                Email = email,
                Topic = topic,
                MessageText = messageText
            };

            _appDbContext.Messages.Add(message);
            _appDbContext.SaveChanges();
        }
    }
}
