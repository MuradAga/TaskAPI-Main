using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using TaskAPI.Context;
using TaskAPI.Entiities;
using TaskAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _appDbContext; // field
        public UsersController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext; // dependency injection object
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_appDbContext.Users.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _appDbContext.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("getByUsernameAndPassword")]
        public User GetByUsernameAndPassword(string username, string password) // Adem123
        {
            SHA256 sha256Hash = SHA256.Create();
            // ComputeHash - returns byte array
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Convert byte array to a string
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return _appDbContext.Users.FirstOrDefault(user => user.Username == username && user.Password == builder.ToString());
        }

        [HttpPost]
        public void Post(User user)
        {
            _appDbContext.Users.Add(user);
            _appDbContext.SaveChanges();
        }

        [HttpPost("register")]
        public IActionResult Register(UserRegisterModel user)
        {
            if (user.Password == user.RepeatPassword)
            {
                try
                {
                    SHA256 sha256Hash = SHA256.Create();
                    // ComputeHash - returns byte array
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(user.Password));

                    // Convert byte array to a string
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }

                    User newUser = new() { Username = user.Username, Password = builder.ToString() };
                    _appDbContext.Users.Add(newUser);
                    _appDbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return BadRequest();
                }

            }
            else
            {
            }
            return Ok();
        }

        [HttpPatch("{id}")]
        public void Put(int id, UserUpdateDTO user) // DTO - Data  Transfer Object
        {
            User updateUser = _appDbContext.Users.FirstOrDefault(user => user.Id == id);
            updateUser.Username = user.Username;
            _appDbContext.SaveChanges();
        }

        [HttpPatch("resetPassword")]
        public void ResetPassword(UserResetPasswordRequestModel model)
        {
            User updateUser = _appDbContext.Users.FirstOrDefault(user => user.Id == model.UserId);

            SHA256 sha256Hash = SHA256.Create();
            // ComputeHash - returns byte array
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(model.OldPassword));

            // Convert byte array to a string
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            if (builder.ToString() == updateUser.Password)
            {
                if (model.NewPassword == model.RepeatNewPassword)
                {
                    builder.Clear();
                    byte[] bytesNewPass = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(model.NewPassword));
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytesNewPass[i].ToString("x2"));
                        updateUser.Password = builder.ToString();
                        _appDbContext.SaveChanges();
                    }
                }
            }
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            User deleteUser = _appDbContext.Users.FirstOrDefault(user => user.Id == id);
            _appDbContext.Users.Remove(deleteUser);
            _appDbContext.SaveChanges();
        }

        [HttpGet("login")]
        public bool Login(string username, string password)
        {
            User user = GetByUsernameAndPassword(username, password);

            if (user != null)
            {
                return true;
            }

            return false;
        }
    }
}
