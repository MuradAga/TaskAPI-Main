using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Security.Cryptography;
using System.Text;
using TaskAPI.Context;
using TaskAPI.Entities;
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
            bool verificationCodeIsTrue = false;
            string verificationCode = _appDbContext.UserEmailCodes.OrderBy(u => u.Id).Last(userEmailCode => userEmailCode.Email == user.PhoneOrEmail).Code;

            if (user.Code == verificationCode)
                verificationCodeIsTrue = true;

            User user2 = _appDbContext.Users.FirstOrDefault(u => u.Username == user.Username);
            if (user2 == null && verificationCodeIsTrue)
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

                    User newUser = new() { FullName = user.FullName, PhoneOrEmail = user.PhoneOrEmail, Username = user.Username, Password = builder.ToString() };
                    _appDbContext.Users.Add(newUser);
                    _appDbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return BadRequest("An error occurred");
                }

                _appDbContext.UserEmailCodes.RemoveRange(_appDbContext.UserEmailCodes.Where(u => u.Email == user.PhoneOrEmail));
                _appDbContext.SaveChanges();
                return Ok();
            }
            else if (!verificationCodeIsTrue)
            {
                return BadRequest("Verification code is false");
            }
            else
            {
                return BadRequest("Username has been used");
            }
        }

        [HttpPost("sendCodeToEmail")]
        public IActionResult SendCodeToEmail(string email)
        {
            string code = new Random().Next(1000, 10000).ToString();
            string subjectText = "Instagram2 Register";
            string bodyText = @$"Verification code: <h1 style='color: red;'>{code}</h1>";

            MimeMessage mimeMessage = new MimeMessage();

            MailboxAddress mailboxAddressFrom = new MailboxAddress("Register", email);
            mimeMessage.From.Add(mailboxAddressFrom);

            MailboxAddress mailboxAddressTo = new MailboxAddress("User", email);
            mimeMessage.To.Add(mailboxAddressTo);

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = bodyText;
            mimeMessage.Body = bodyBuilder.ToMessageBody();
            mimeMessage.Subject = subjectText;

            SmtpClient client = new SmtpClient();
            client.Connect("smtp.gmail.com", 587, false);
            client.Authenticate("petopiaanimal@gmail.com", "ldfcaeeotzoqzgih");
            client.Send(mimeMessage);
            client.Disconnect(true);

            _appDbContext.UserEmailCodes.Add(new UserEmailCode() { Email = email, Code = code });
            _appDbContext.SaveChanges();

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

        [HttpPatch("forgotPassword")]
        public IActionResult ForgotPassword(UserForgotPasswordRequestModel model)
        {
            if (model.NewPassword == model.RepeatNewPassword)
            {
                bool verificationCodeIsTrue = false;
                string verificationCode = _appDbContext.UserEmailCodes.OrderBy(u => u.Id).Last(userEmailCode => userEmailCode.Email == model.Email).Code;

                if (model.VerificationCode == verificationCode)
                    verificationCodeIsTrue = true;

                if (verificationCodeIsTrue)
                {
                    SHA256 sha256Hash = SHA256.Create();
                    // ComputeHash - returns byte array
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(model.NewPassword));

                    // Convert byte array to a string
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }

                    User user = _appDbContext.Users.FirstOrDefault(user => user.PhoneOrEmail == model.Email);
                    user.Password = builder.ToString();

                    _appDbContext.UserEmailCodes.RemoveRange(_appDbContext.UserEmailCodes.Where(u => u.Email == user.PhoneOrEmail));
                    _appDbContext.SaveChanges();
                    return Ok();
                }
                else
                {
                    return BadRequest("Verification code is false");
                }
            }
            else
            {
                return BadRequest("Passwords is not equal");
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
        public IActionResult Login(string username, string password)
        {
            User user = GetByUsernameAndPassword(username, password); // null

            if (user == null)
                return NotFound();

            return Ok(user);
        }
    }
}
