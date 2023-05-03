namespace ElearnBackEnd.Model
{
    public class Author :BaseEntity
    {
        public string FullName { get; set; }
        public string AuthorImage { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}
