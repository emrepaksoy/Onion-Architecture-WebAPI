using BooksBazaarAPI.Domain.Entities.Common;


namespace BooksBazaarAPI.Domain.Entities
{
    public class Book : BaseEntity
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public float Rating { get; set; }
        public string Explanation { get; set; }
        public BookImageFile BookImageFile { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
