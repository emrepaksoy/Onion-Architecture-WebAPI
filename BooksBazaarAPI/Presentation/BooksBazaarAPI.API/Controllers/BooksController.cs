using BooksBazaarAPI.Application.Abstraction.Services;
using BooksBazaarAPI.Application.DTOs.Book;
using BooksBazaarAPI.Application.Features.Commands.Book.CreateBook;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BooksBazaarAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        readonly IMediator _mediator;
        readonly IBookService _bookService;
        public BooksController(IMediator mediator, IBookService bookService)
        {
            _mediator = mediator;
            _bookService = bookService;
        }

        [HttpGet("[Action]")]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _bookService.GetAllBookAsync();
            return Ok(books);
        }

        [HttpGet("[Action]/{id}")]
        public async Task<IActionResult> GetBook(string id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            return Ok(book);
        }
       
        [HttpPost("[Action]")]
        public async Task<IActionResult> CreateBook(CreateBookDTO bookModel)
        {
            await _bookService.CreateBookAsync(bookModel);

            return Ok();
        }

        //[HttpPost("[Action]")]
        //public async Task<IActionResult> CreateBook(CreateBookCommandRequest createBookCommandRequest)
        //{
        //    CreateBookCommandResponse response = await _mediator.Send(createBookCommandRequest);
        //    return Ok(response);

        //}

        [HttpDelete("[Action]/{id}")]

        public async Task<IActionResult> DeleteBook(string id)
        {
            return Ok(await _bookService.DeleteBookAsync(id));
        }
    }
}
