namespace TaskAPI.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Topic { get; set; }
        public string MessageText { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}