using BooksBazaarAPI.Application.DTOs.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksBazaarAPI.Application.Abstraction.Services
{
    public interface ICommentService
    {
        Task CreateCommentAsync(CreateCommentDTO createCommentDTO);
        Task<ListCommentDTO> GetAllCommentsAsync(string bookId);

        Task DeleteCommentAsync(string bookId);
    }
}
