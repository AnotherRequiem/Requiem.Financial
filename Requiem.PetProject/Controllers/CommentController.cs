using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Requiem.PetProject.DTOs.Comment;
using Requiem.PetProject.Extensions;
using Requiem.PetProject.Helpers;
using Requiem.PetProject.Interfaces;
using Requiem.PetProject.Mappers;
using Requiem.PetProject.Models;

namespace Requiem.PetProject.Controllers;

[Route("api/comment")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;
    private readonly IStockRepository _stockRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly IFmpService _fmpService;

    public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository,
        UserManager<AppUser> userManager, IFmpService fmpService)
    {
        _commentRepository = commentRepository;
        _stockRepository = stockRepository;
        _userManager = userManager;
        _fmpService = fmpService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] CommentQueryObject commentQuery)
    {
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);
        
        var comments = await _commentRepository.GetAllAsync(commentQuery);

        var commentDto = comments.Select(c => c.ToCommentDto());

        return Ok(commentDto);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);
        
        var comment = await _commentRepository.GetByIdAsync(id);

        if (comment == null) 
            return NotFound();

        return Ok(comment.ToCommentDto());
    }

    [HttpPost]
    [Route(("{symbol:alpha}"))]
    public async Task<IActionResult> Create([FromRoute] string symbol, CreateCommentDto commentDto)
    {
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);

        var stock = await _stockRepository.GetBySymbolAsync(symbol);

        if (stock == null)
        {
            stock = await _fmpService.FindStockSymbolAsync(symbol);

            if (stock == null)
                return BadRequest("Stock does not exists");
            else
                await _stockRepository.CreateAsync(stock);
        }

        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);

        var commentModel = commentDto.ToCommentFromCreateDto(stock.Id);
        commentModel.AppUserId = appUser.Id;

        await _commentRepository.CreateAsync(commentModel);

        return CreatedAtAction(nameof(GetById), new {id = commentModel.Id}, commentModel.ToCommentDto());
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCommentRequestDto updateDto)
    {
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);
        
        var comment = await _commentRepository.UpdateAsync(id, updateDto.ToCommentFromUpdateDto(id));

        if (comment == null) 
            return NotFound("Comment not found");

        return Ok(comment.ToCommentDto());
    }
    
    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);
        
        var commentModel = await _commentRepository.DeleteAsync(id);

        if (commentModel == null) 
            return NotFound("Comment does not exist");
        
        return Ok(commentModel);
    } 
}