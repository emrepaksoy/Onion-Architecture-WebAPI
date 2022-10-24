using BooksBazaarAPI.Application.Abstraction.Services;
using BooksBazaarAPI.Application.DTOs.Comment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BooksBazaarAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        //[HttpGet("[Action]/{id}")]
        //public


        [HttpPost("[Action]")]
        public async Task<IActionResult> CreateComment(CreateCommentDTO createCommentDTO)
        {
            await _commentService.CreateCommentAsync(createCommentDTO);
            return Ok();
        }
    }
}
