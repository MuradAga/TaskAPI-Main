namespace TaskAPI.Models
{
    public class UserRegisterModel
    {
        public string PhoneOrEmail { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }
    }
}
