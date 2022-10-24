using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksBazaarAPI.Application.DTOs.Book
{
    public class CreateBookDTO
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Explanation { get; set; }
        
    }
}
