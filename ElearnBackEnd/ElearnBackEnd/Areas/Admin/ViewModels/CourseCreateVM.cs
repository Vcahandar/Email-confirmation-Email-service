using System.ComponentModel.DataAnnotations;

namespace ElearnBackEnd.Areas.Admin.ViewModels
{
    public class CourseCreateVM
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Price { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public int AuthorId { get; set; }
        [Required]
        public List<IFormFile> Photos { get; set; }

    }
}
