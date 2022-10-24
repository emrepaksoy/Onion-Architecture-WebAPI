using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksBazaarAPI.Application.DTOs.Comment
{
    public class ListCommentDTO
    {
        public int TotalCommentCount { get; set; }
        public object Comments { get; set; }
    }
}
