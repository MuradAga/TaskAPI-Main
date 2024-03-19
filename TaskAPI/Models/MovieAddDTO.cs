using TaskAPI.Entities;

namespace TaskAPI.Models
{
    public class MovieAddDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CoverUrl { get; set; }
        public string TrailerUrl { get; set; }
        public ushort Year { get; set; }
        public bool Language { get; set; }
        public float ImdbPoint { get; set; }
        public List<int> CategoryIds { get; set; }
        public List<int> DirectorIds { get; set; }
        public List<int> ActorIds { get; set; }
    }
}
