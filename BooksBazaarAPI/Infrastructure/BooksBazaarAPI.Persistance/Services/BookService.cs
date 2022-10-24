using BooksBazaarAPI.Application.Abstraction.Services;
using BooksBazaarAPI.Application.DTOs.Book;
using BooksBazaarAPI.Application.Repositories;
using BooksBazaarAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksBazaarAPI.Persistance.Services
{
    public class BookService : IBookService
    {

        readonly IBookReadRepository   _bookReadRepository;
        readonly IBookWriteRepository _bookWriteRepository;
        public BookService(IBookReadRepository bookReadRepository, IBookWriteRepository bookWriteRepository)
        {
            _bookReadRepository = bookReadRepository;
            _bookWriteRepository = bookWriteRepository;
        }

        public async Task CreateBookAsync(CreateBookDTO createBook)
        {
            await _bookWriteRepository.AddAsync(new()
            {
                Author = createBook.Author,
                Name = createBook.Name,
                Explanation = createBook.Explanation

            }
            );
            await _bookWriteRepository.SaveAsync();
        }

        public async Task<bool> DeleteBookAsync(string id)
        {
            var s = await _bookWriteRepository.RemoveAsync(id);
            await _bookWriteRepository.SaveAsync();

            return s;
        }

        public async Task<ListBookDTO> GetAllBookAsync()
        {
            var totalProductCount =  _bookReadRepository.GetAll(false).Count();
            var books =  _bookReadRepository.GetAll(false).Include(p => p.BookImageFile).Select( p => new
            {
                p.Id,
                p.Name,
                p.Author,
                p.Explanation,
                p.Rating,
                p.CreatedDate,
                p.BookImageFile,
            });

            return new()
            {
                TotalBookCount = totalProductCount,
                Books = books.ToList()
            };


        }

        public async Task<SingleBook> GetBookByIdAsync(string id)
        {
            Book book = await  _bookReadRepository.GetByIdAsync(id,false);

            return new()
            {
                Id = book.Id,
                Author = book.Author,
               Name=book.Name,
               Rating = book.Rating,
               Explanation=book.Explanation,
               CreatedDate = book.CreatedDate,

            };
        }
    }
}
