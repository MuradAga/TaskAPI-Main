namespace TaskAPI.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CoverUrl { get; set; }
        public string TrailerUrl { get; set; }
        public ushort Year { get; set; }
        public bool Language { get; set; }
        public uint Views { get; set; } = 0;
        public float ImdbPoint { get; set; }
        public List<Category> Categories { get; set; }
        public List<Director> Directors { get; set; }
        public List<Actor> Actors { get; set; }
    }
}
