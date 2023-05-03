namespace ElearnBackEnd.Model
{
    public class News : BaseEntity
    {
        public string Title { get; set; }

        public string Image { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
