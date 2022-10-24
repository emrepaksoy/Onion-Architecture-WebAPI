using BooksBazaarAPI.Application.Abstraction.Services;
using BooksBazaarAPI.Application.DTOs.Comment;
using BooksBazaarAPI.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksBazaarAPI.Persistance.Services
{
    public class CommentService : ICommentService
    {
        readonly ICommentReadRepository _commentReadRepository;
        readonly ICommentWriteRepository _commentWriteRepository;

        public CommentService(ICommentWriteRepository commentWriteRepository, ICommentReadRepository commentReadRepository)
        {
            _commentWriteRepository = commentWriteRepository;
            _commentReadRepository = commentReadRepository;
        }

        public async Task CreateCommentAsync(CreateCommentDTO createCommentDTO)
        {
            await _commentWriteRepository.AddAsync(new()
            {
                Content = createCommentDTO.Content,
                UserId = createCommentDTO.AppUserId,
                BookId = createCommentDTO.BookId


            });
            await _commentWriteRepository.SaveAsync();
        }

        public Task DeleteCommentAsync(string bookId)
        {
            throw new NotImplementedException();
        }

        public Task<ListCommentDTO> GetAllCommentsAsync(string bookId)
        {
            throw new NotImplementedException();
        }
    }
}
