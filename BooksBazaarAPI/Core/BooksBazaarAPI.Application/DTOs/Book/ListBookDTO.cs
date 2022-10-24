using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksBazaarAPI.Application.DTOs.Book
{
    public class ListBookDTO
    {
        public int TotalBookCount { get; set; }
        public object Books { get; set; }
    }
}
