using BooksBazaarAPI.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;


namespace BooksBazaarAPI.Domain.Entities
{
    public class BookImageFile : BaseEntity
    {
        public string FileName { get; set; }
        public string Path { get; set; }

        public Guid BookId { get; set; }
        public string Storage { get; set; }
        public Book Book { get; set; }

        [NotMapped]
        public override DateTime UpdatedDate { get => base.UpdatedDate; set => base.UpdatedDate = value; }

    }
}
