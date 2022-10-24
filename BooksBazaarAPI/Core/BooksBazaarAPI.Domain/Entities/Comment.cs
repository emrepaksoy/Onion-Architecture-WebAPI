
using BooksBazaarAPI.Domain.Entities.Common;
using BooksBazaarAPI.Domain.Entities.Identity;

namespace BooksBazaarAPI.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        public string UserId { get; set; }
        public Guid BookId { get; set; }
        public AppUser User { get; set; }
        public Book Book { get; set; }


    }
}
