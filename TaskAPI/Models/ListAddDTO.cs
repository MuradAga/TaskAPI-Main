using TaskAPI.Entities;

namespace TaskAPI.Models
{
    public class ListAddDTO
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public List<int> MovieIds { get; set; }
    }
}
