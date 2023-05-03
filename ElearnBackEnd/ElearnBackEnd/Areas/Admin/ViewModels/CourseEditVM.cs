using ElearnBackEnd.Model;
using System.ComponentModel.DataAnnotations;

namespace ElearnBackEnd.Areas.Admin.ViewModels
{
    public class CourseEditVM
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public string Price { get; set; }
        [Required]
        public int AuthorId { get; set; }
        public ICollection<CourseImage> CourseImages { get; set; }
        public List<IFormFile> Photos { get; set; }
    }
}
