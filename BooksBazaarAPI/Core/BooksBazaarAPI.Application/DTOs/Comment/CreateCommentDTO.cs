using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksBazaarAPI.Application.DTOs.Comment
{
    public class CreateCommentDTO
    {
        public string Content { get; set; }
        public string AppUserId { get; set; }
        public Guid BookId { get; set; }

    }
}
