namespace ElearnBackEnd.Model
{
    public class CourseImage :BaseEntity
    {
        public string Image { get; set; }
        public bool IsMain { get; set; } = false;
        public int CourseId { get; set; }
        public Course Courses { get; set; }

    }
}
