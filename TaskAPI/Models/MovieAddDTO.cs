using TaskAPI.Entities;

namespace TaskAPI.Models
{
    public class MovieAddDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ushort Year { get; set; }
        public bool Language { get; set; }
        public float ImdbPoint { get; set; }
        public List<Category> Categories { get; set; }
        public List<Director> Directors { get; set; }
        public List<Actor> Actors { get; set; }
    }
}
