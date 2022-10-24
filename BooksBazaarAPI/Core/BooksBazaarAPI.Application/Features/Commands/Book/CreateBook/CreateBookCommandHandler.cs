using BooksBazaarAPI.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksBazaarAPI.Application.Features.Commands.Book.CreateBook
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommandRequest, CreateBookCommandResponse>
    {
        readonly IBookWriteRepository _bookWriteRepository;

        public CreateBookCommandHandler(IBookWriteRepository bookWriteRepository)
        {
            _bookWriteRepository = bookWriteRepository;
        }

        public async Task<CreateBookCommandResponse> Handle(CreateBookCommandRequest request, CancellationToken cancellationToken)
        {
            await _bookWriteRepository.AddAsync(new()
            {
                Name = request.CreateBookModel.Name,
                Author = request.CreateBookModel.Author,
                Explanation = request.CreateBookModel.Explanation,

            }) ;

            await _bookWriteRepository.SaveAsync();
            return new();

        }
    }
}
