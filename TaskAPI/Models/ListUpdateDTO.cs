namespace TaskAPI.Models
{
    public class ListUpdateDTO
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public List<int> MovieIds { get; set; }
    }
}
