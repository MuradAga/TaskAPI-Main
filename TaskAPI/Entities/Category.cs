using System.ComponentModel.DataAnnotations;

namespace TaskAPI.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Movie> Movies { get; set; }
    }
}
