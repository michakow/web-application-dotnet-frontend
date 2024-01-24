using System.ComponentModel.DataAnnotations;

namespace Frontend.Models
{
    public class BookModel
    {
        [Key]
        public string? BookId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Author { get; set; }
        public string? Series { get; set; }
        public List<string>? Genres { get; set; }
        public DateTime? Published { get; set; }
    }
}
