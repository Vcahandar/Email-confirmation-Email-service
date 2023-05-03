namespace ElearnBackEnd.Model
{
    public class Course : BaseEntity
    {
        
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int SaleCount { get; set; }
        public ICollection<CourseImage> Images { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
