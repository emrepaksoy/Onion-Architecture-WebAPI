using BooksBazaarAPI.Application.DTOs.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksBazaarAPI.Application.Abstraction.Services
{
    public interface IBookService
    {
        Task CreateBookAsync(CreateBookDTO createBook);
        Task<ListBookDTO> GetAllBookAsync();

        Task<SingleBook> GetBookByIdAsync(string id);

        Task<bool> DeleteBookAsync(string id);
    }
}
