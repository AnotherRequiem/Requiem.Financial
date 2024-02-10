using Microsoft.AspNetCore.Mvc;
using Requiem.PetProject.Interfaces;
using Requiem.PetProject.Mappers;

namespace Requiem.PetProject.Controllers;

[Route("api/comment")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;

    public CommentController(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var comments = await _commentRepository.GetAllAsync();

        var commentDto = comments.Select(c => c.ToCommentDto());

        return Ok(commentDto);
    }
}