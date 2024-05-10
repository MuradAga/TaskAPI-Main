namespace TaskAPI.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string PhoneOrEmail { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}