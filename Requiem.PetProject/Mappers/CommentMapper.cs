using Requiem.PetProject.DTOs.Comment;
using Requiem.PetProject.Models;

namespace Requiem.PetProject.Mappers;

public static class CommentMapper
{
    public static CommentDto ToCommentDto(this Comment commentModel)
    {
        return new CommentDto()
        {
            Id = commentModel.Id,
            Title = commentModel.Title,
            Content = commentModel.Content,
            CreatedOn = commentModel.CreatedOn,
            CreatedBy = commentModel.AppUser.UserName,
            StockId = commentModel.StockId
        };
    }
    
    public static Comment ToCommentFromCreateDto(this CreateCommentDto commentDto, Guid stockId)
    {
        return new Comment()
        {
            Title = commentDto.Title,
            Content = commentDto.Content,
            StockId = stockId
        };
    }
    
    public static Comment ToCommentFromUpdateDto(this UpdateCommentRequestDto commentDto, Guid stockId)
    {
        return new Comment
        {
            Title = commentDto.Title,
            Content = commentDto.Content,
            StockId = stockId
        };
    }
}