namespace TaskAPI.Entities
{
    public class List
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public List<Movie> Movies { get; set; }
    }
}
